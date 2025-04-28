using CustomerManagement.Data;
using CustomerManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class FeeRecordController : Controller
{
    private readonly ApplicationDbContext _context;

    public FeeRecordController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult FeeTracking()
    {
        // Get latest version of each customer (based on MasterCustomerId)
        var latestCustomers = _context.Customers
            .GroupBy(c => c.MasterCustomerId)
            .Select(g => g.OrderByDescending(c => c.Id).FirstOrDefault())
            .ToList();

        // Get fee records
        var feeRecords = _context.FeeRecords.ToList();

        // Map to view model with fees paid calculated from FeeHistories
        var feeSummaryList = (from record in feeRecords
                              join customer in latestCustomers
                                  on record.MasterCustomerId equals customer.MasterCustomerId
                              let feesPaid = _context.FeeHistories
                                  .Where(fh => fh.MasterCustomerId == record.MasterCustomerId &&
                                               fh.FinancialYear == record.FinancialYear)
                                  .Sum(fh => (decimal?)fh.FeesPaid) ?? 0
                              select new FeeSummaryViewModel
                              {
                                  FeeRecordId = record.Id,
                                  MasterCustomerId = record.MasterCustomerId,
                                  CustomerName = customer.CustomerName,
                                  FinancialYear = record.FinancialYear,
                                  TotalFees = record.TotalFees,
                                  FeesPaid = feesPaid
                                  
                              }).ToList();

        ViewBag.FeeSummaryList = feeSummaryList;
        ViewBag.Customers = latestCustomers;
        ViewBag.FinancialYears = GetFinancialYears();

        return View();
    }

    [HttpPost]
    public IActionResult AddFees(int masterCustomerId, string financialYear, decimal totalFees)
    {
        if (string.IsNullOrEmpty(financialYear) || totalFees <= 0)
        {
            TempData["ErrorMessage"] = "Please provide valid Financial Year and Total Fees.";
            return RedirectToAction("FeeTracking");
        }

        var latestCustomer = _context.Customers
            .Where(c => c.MasterCustomerId == masterCustomerId)
            .OrderByDescending(c => c.Id)
            .FirstOrDefault();

        if (latestCustomer == null)
        {
            TempData["ErrorMessage"] = "Customer not found.";
            return RedirectToAction("FeeTracking");
        }

        // Check if a FeeRecord already exists for the given MasterCustomerId and FinancialYear
        var existingRecord = _context.FeeRecords
            .FirstOrDefault(fr => fr.MasterCustomerId == masterCustomerId && fr.FinancialYear == financialYear);

        if (existingRecord != null)
        {
            // If a record exists, show an error message and instruct the user to click Edit
            TempData["ErrorMessage"] = "Fee record already exists. Kindly click on Edit to update the fees.";
        }
        else
        {
            // If no record exists, create a new one
            var newRecord = new FeeRecord
            {
                CustomerId = latestCustomer.Id,
                MasterCustomerId = masterCustomerId,
                FinancialYear = financialYear,
                TotalFees = totalFees,
                UpdatedOn = DateTime.Now
            };

            _context.FeeRecords.Add(newRecord);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Total Fees added successfully.";
        }

        return RedirectToAction("FeeTracking");
    }

    [HttpPost]
    public IActionResult EditFeeRecord(int feeRecordId, decimal totalFees)
    {
        var feeRecord = _context.FeeRecords.FirstOrDefault(fr => fr.Id == feeRecordId);
        if (feeRecord == null)
        {
            TempData["ErrorMessage"] = "Fee record not found.";
            return RedirectToAction("FeeTracking");
        }

        feeRecord.TotalFees = totalFees;
        feeRecord.UpdatedOn = DateTime.Now;

        _context.SaveChanges();
        TempData["SuccessMessage"] = "Total Fees updated successfully.";
        return RedirectToAction("FeeTracking");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CollectFees(int FeeRecordId, decimal FeesPaid, DateTime PaidOn)
    {
        // Fetch the FeeRecord from FeeRecords using FeeRecordId
        var feeRecord = _context.FeeRecords.FirstOrDefault(fr => fr.Id == FeeRecordId);
        if (feeRecord == null)
        {
            TempData["ErrorMessage"] = "Fee record not found.";
            return RedirectToAction("FeeTracking");
        }

        // Get the MasterCustomerId from the FeeRecord
        int masterCustomerId = feeRecord.MasterCustomerId;

        // Check if the MasterCustomerId exists in the Customers table
        var customerExists = _context.Customers.Any(c => c.MasterCustomerId == masterCustomerId);
        if (!customerExists)
        {
            TempData["ErrorMessage"] = "Customer not found for the given MasterCustomerId.";
            return RedirectToAction("FeeTracking");
        }

        // Add the new fee history record
        var newFee = new FeeHistory
        {
            MasterCustomerId = masterCustomerId,  // Use the MasterCustomerId from FeeRecord
            FinancialYear = feeRecord.FinancialYear,  // Using the same financial year from FeeRecord
            FeesPaid = FeesPaid,
            UpdatedOn = PaidOn,  // Use the paid date from the form
        };

        // Add to FeeHistories table
        _context.FeeHistories.Add(newFee);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Fees collected successfully.";
        return RedirectToAction("FeeTracking");
    }

    [HttpGet]
    public JsonResult GetFeeHistories(int masterCustomerId, string financialYear)
    {
        var feeHistories = _context.FeeHistories
            .Where(fh => fh.MasterCustomerId == masterCustomerId && fh.FinancialYear == financialYear)
            .OrderBy(fh => fh.UpdatedOn)
            .Select(fh => new
            {
                date = fh.UpdatedOn.ToString("dd-MM-yyyy"),
                amount = fh.FeesPaid
            })
            .ToList();

        return Json(feeHistories);
    }

    private List<string> GetFinancialYears()
    {
        int startYear = DateTime.Now.Year - 5;
        return Enumerable.Range(startYear, 10)
                         .Select(y => $"{y}-{y + 1}")
                         .ToList();
    }
}
