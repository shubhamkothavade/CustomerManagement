$(document).ready(function () {
    let isVisible = false;

    $("#toggleCustomers").click(function () {
        const tableContainer = $("#customerTableContainer");
        const icon = $(this).find("i");
        const text = $(this).find(".visually-hidden");

        if (isVisible) {
            tableContainer.slideUp(300);
            icon.removeClass("fa-chevron-up").addClass("fa-chevron-down");
            text.text("Show Customers");
        } else {
            tableContainer.slideDown(300);
            icon.removeClass("fa-chevron-down").addClass("fa-chevron-up");
            text.text("Hide Customers");
        }

        isVisible = !isVisible;
    });
});

$(document).ready(function () {
    let formVisible = false;

    $("#toggleForm").click(function () {
        const formBox = $("#customerFormBox");
        const icon = $(this).find("i");
        const text = $(this).find(".visually-hidden");

        if (formVisible) {
            formBox.slideUp(300);
            icon.removeClass("fa-chevron-up").addClass("fa-chevron-down");
            text.text("Show Form");
        } else {
            formBox.slideDown(300);
            icon.removeClass("fa-chevron-down").addClass("fa-chevron-up");
            text.text("Hide Form");
        }

        formVisible = !formVisible;
    });
});

