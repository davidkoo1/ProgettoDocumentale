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

function handleSubmitButton(formId) {
    var $form = $(formId);
    if ($form.length === 0) return; // Проверяем, существует ли форма

    $.ajax({
        type: $form.attr('method'),
        url: $form.attr('action'),
        data: $form.serialize(),
        success: function (response) {
            if (response.success) {
                $('#lgModal').modal('hide');
                $('#UserDatatable').DataTable().ajax.reload(null, false);
                $('#InstitutionDatatable').DataTable().ajax.reload(null, false);
            } else {
                $('.modal-body').html(response);
                $('.selectpicker').selectpicker('refresh');
            }
        },
        error: function (xhr, status, error) {
            alert("Произошла ошибка: " + error);
        }
    });
}