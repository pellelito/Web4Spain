///
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//close contactModal
$(document).ready(function () {
    //Show contact modal
    $("#btnLink").click(function () {
        $('#contactModal').modal('show');
    })
    //Close the contact modal
    $("#btnHideModal").click(function () {
        $("#contactModal").modal('hide');
    });
    $(".datepicker").datepicker({
        dateFormat: "dd-mm-yy",
        changemonth: true,
        changeyear: true
    });
    
});


