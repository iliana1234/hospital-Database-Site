// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".clickable-row").forEach(row => {
        row.addEventListener("click", function (e) {
            // This will prevent from the row to be clicked when clicking on the other links in it
            if (!e.target.closest("a")) {
                window.location = this.dataset.href;
            }
        });
    });
});
