function drawPatrialView(url, divId) {
    $.ajax({
        url: url,
        cache: false,
        type: "GET",
        dataType: "html",
        statusCode: {
            302: function (data) {
                window.location.href = '/Account/Logout/';
            },
        },
        success: function (result, e) {
            $('#' + divId).html(result);
        },
    });
}

function handleSubmitButton(formId, reloadDataTableId) {
    var $form = $(formId);
    if ($form.length === 0) return; // Check if the form exists

    var formData = new FormData($form[0]); // Create FormData object from form

    $.ajax({
        type: $form.attr('method'),
        url: $form.attr('action'),
        data: formData, // Use FormData object as the data
        processData: false, // Prevent jQuery from processing the data
        contentType: false, // Prevent jQuery from setting the content type
        success: function (response) {
            if (response.success) {
                $('#lgModal').modal('hide');
                $('#' + reloadDataTableId).DataTable().ajax.reload(null, false);
                if (reloadDataTableId === "ProjectDatatable") {
                    initializeProjectThree();
                }
            } else {
                $('.modal-body').html(response);
                $('.selectpicker').selectpicker('refresh');
            }
        },
        error: function (xhr, status, error) {
            alert("An error occurred: " + error);
        }
    });
}






