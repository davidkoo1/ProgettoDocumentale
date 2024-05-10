//UserData

function initializeUserDataTable() {
    var table = $('#UserDatatable').DataTable({
        //"processing": true,
        "serverSide": true,
        //"scrollX": true,
        //"autoWidth": true,
        //"stateSave": true,
        //"dom": "p", //f, r, t, l, p
        ajax: {
            "url": "/User/LoadDatatable",
            "type": "POST",
            "dataType": "json",
        },
        "columns": [
            { "data": "Id", "title": "Id", "name": "Id", "visible": false },
            { "data": "UserName", "title": "UserName", "name": "UserName", "autoWidth": true },
            { "data": "Email", "title": "Email", "name": "Email", "autoWidth": true },
            { "data": "IsEnabled", "title": "IsEnabled", "name": "IsEnabled", "autoWidth": true }
        ],
        columnDefs: [
            { className: 'text-center', targets: [3] },
        ],
    });

    var contextmenu = $('#UserDatatable').contextMenu({
        selector: 'tr',
        trigger: 'right',
        callback: function (key, options) {
            let row = table.row(options.$trigger);
            switch (key) {
                case 'Edit':
                    $('#lgModal').modal('show');
                    drawPatrialView('../User/Edit/' + row.data()["Id"] , 'lgModalBody');
                    break;
                case 'Details':
                    $('#lgModal').modal('show');
                    drawPatrialView('../User/Details/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Delete':
                    $('#lgModal').modal('show');
                    drawPatrialView('../User/Delete/' + row.data()["Id"], 'lgModalBody');
                    break;
                default:
                    break
            }
        },
        items: {
            "Edit": { name: "Edit" },
            "Details": { name: "Details" },
            "Delete": { name: "Delete" }
        }
    });

    return table;
}

function deleteCurrentUser(userId) {
    $.ajax({
        url: '../User/DeleteConfirmed/' + userId,
        cache: false,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.success) {
                $('#UserDatatable').DataTable().ajax.reload(null, false);
            } 
        }
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
