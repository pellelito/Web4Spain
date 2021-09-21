// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//close contactModal
$(document).ready(function () {
    $("#btnLink").click(function () {
        $('#contactModal').modal('show');
    })
    $("#btnHideModal").click(function () {
        $("#contactModal").modal('hide');
    });
});