//UserData
function setupRowClickEvents(table) {
    $('#UserDatatable tbody').on('click', 'tr', function () {
        var rowData = table.row(this).data();
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $('#actions').hide();
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            updateButtonUserLinks(rowData.Id);
            checkTableDataAndToggleActions(table);
        }
    });
    
}
function checkTableDataAndToggleActions(table) {
    if (table.rows().count() === 0) {
        $('#actions').hide();
    } else {
        $('#actions').show();
    }
}
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
            "beforeSend": function () {
                $('#actions').hide();
            }
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

    return table;
}

function updateButtonUserLinks(id) {

    $('#editLink').attr('onclick', `drawPatrialView('/User/Edit/'+${id}, 'lgModalBody')`);
    $('#detailsLink').attr('onclick', `drawPatrialView('/User/Details/'+${id}, 'lgModalBody')`);
    $('#deleteLink').attr('onclick', `drawPatrialView('/User/Delete/'+${id}, 'lgModalBody')`);
}

function deleteCurrentUser(userId) {
    $.ajax({
        url: '../User/DeleteConfirmed/' + userId,
        cache: false,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.success) {
                $('#actions').hide();
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
                $('#actions').hide();
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
