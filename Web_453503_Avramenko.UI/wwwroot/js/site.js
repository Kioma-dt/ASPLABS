// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).on("click", ".ajax-pagination a.page-link", function (event) {
    event.preventDefault();

    const link = $(this);
    const container = link.closest(".ajax-pagination");
    const url = link.attr("href");

    if (url) {
        container.load(url);
    }
});
