using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerManagement.Models;
using CustomerManagement.Data;

namespace CustomerManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Login", "Login");

            var latestCustomers = _context.Customers
                .AsEnumerable() // Client-side grouping
                .GroupBy(c => c.MasterCustomerId)
                .Select(g => g.OrderByDescending(c => c.CreatedAt).First())
                .ToList();

            return View(latestCustomers);
        }

        public IActionResult Create()
        {
            return View(new Customer());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedAt = DateTime.Now;
                _context.Customers.Add(customer);
                _context.SaveChanges();

                customer.MasterCustomerId = customer.Id;
                _context.SaveChanges();

                return Json(new { success = true });
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, message = string.Join("\n", errors) });
        }

        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            // Fetch fee history based on MasterCustomerId (no longer fetching financial data here)
            //var feeHistories = _context.FeeHistories
            //    .Where(f => f.CustomerId == customer.Id || _context.Customers
            //        .Where(c => c.MasterCustomerId == customer.MasterCustomerId)
            //        .Select(c => c.Id)
            //        .Contains(f.CustomerId))
            //    .OrderByDescending(f => f.UpdatedOn)
            //    .ToList();

            //ViewBag.FeeHistories = feeHistories;

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer updatedCustomer)
        {
            var existingCustomer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (existingCustomer == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingCustomer.CustomerName = updatedCustomer.CustomerName;
                existingCustomer.ContactPerson = updatedCustomer.ContactPerson;
                existingCustomer.PhoneNo = updatedCustomer.PhoneNo;
                existingCustomer.MobileNo = updatedCustomer.MobileNo;
                existingCustomer.Address1 = updatedCustomer.Address1;
                existingCustomer.Address2 = updatedCustomer.Address2;
                existingCustomer.Service = updatedCustomer.Service;
                existingCustomer.Notes = updatedCustomer.Notes;
                existingCustomer.PanCardNo = updatedCustomer.PanCardNo;
                existingCustomer.GSTNO = updatedCustomer.GSTNO;

                _context.Update(existingCustomer);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            // TEMP: Print out validation errors
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Field: {state.Key} - Error: {error.ErrorMessage}");
                }
            }

            return View(updatedCustomer);
        }
        // This is the GET method for confirming deletion (no changes needed here)
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer); // Display the customer details on the delete confirmation page
        }

        // This is the POST method for actually deleting the customer (renamed for clarity)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            // Remove related FeeHistories
            var feeHistories = _context.FeeHistories
                .Where(fh => fh.MasterCustomerId == customer.MasterCustomerId);
            _context.FeeHistories.RemoveRange(feeHistories);

            // Remove related FeeRecords
            var feeRecords = _context.FeeRecords
                .Where(fr => fr.MasterCustomerId == customer.MasterCustomerId);
            _context.FeeRecords.RemoveRange(feeRecords);

            // Remove the customer
            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        public IActionResult Details(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
    }
}
