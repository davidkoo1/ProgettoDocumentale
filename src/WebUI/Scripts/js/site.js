//UserData
function setupRowClickEvents(table) {
    $('#UserDatatable tbody').on('click', 'tr', function () {
        debugger;
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
        "processing": true,
        "serverSide": true,
        "scrollX": true,
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
            { "data": "UserName", "title": "UserName", "name": "UserName" },
            { "data": "Email", "title": "Email", "name": "Email" },
            { "data": "IsEnabled", "title": "IsEnabled", "name": "IsEnabled" }
        ]
    });

    return table;
}

function updateButtonUserLinks(id) {
    $('#editLink').attr('onclick', `drawPatrialView('/User/GetUpsert/'+${id}, 'xlModalBody')`);
    $('#detailsLink').attr('onclick', `drawPatrialView('/User/Details/'+${id}, 'lgModalBody')`);
    $('#deleteLink').attr('onclick', `deleteUser(${id})`);
}

function drawPatrialView(url, divId) {
    debugger;
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
