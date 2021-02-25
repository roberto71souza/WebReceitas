// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
ShowInModal = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#exampleModal .modal-body").html(res);
            $("#exampleModal .modal-title").html(title);
            $("#exampleModal").modal('show');
        }
    });
};

funcReload = () => {
location.reload();
};
