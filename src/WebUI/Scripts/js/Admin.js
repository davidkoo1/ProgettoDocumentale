
//InstitutionData
function initializeInstitutionDataTable() {
    var table = $('#InstitutionDatatable').DataTable({
        //"processing": true,
        "serverSide": true,
        //"scrollX": true,
        //"autoWidth": true,
        //"stateSave": true,
        //"dom": "p", //f, r, t, l, p
        ajax: {
            "url": "/Institution/LoadInstitutionDatatable",
            "type": "POST",
            "dataType": "json",
        },
        "columns": [
            { "data": "Id", "title": "Id", "name": "Id", "visible": false },
            { "data": "InstCode", "title": "InstCode", "name": "InstCode", "autoWidth": true },
            { "data": "Name", "title": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "AdditionalInfo",
                "title": "AdditionalInfo",
                "name": "AdditionalInfo",
                "autoWidth": true,
                "render": function (data, type, row) {
                    if (data.length > 20) {
                        return data.substr(0, 20) + '...';
                    } else {
                        return data;
                    }
                }
            }
        ],
        columnDefs: [
            { className: 'text-center', targets: [3] },
        ],
    });

    var contextmenu = $('#InstitutionDatatable').contextMenu({
        selector: 'tr',
        trigger: 'right',
        callback: function (key, options) {
            let row = table.row(options.$trigger);
            switch (key) {
                case 'Edit':
                    $('#lgModal').modal('show');
                    drawPatrialView('../Institution/GetEdit/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Details':
                    $('#lgModal').modal('show');
                    drawPatrialView('../Institution/GetDetails/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Delete':
                    $('#lgModal').modal('show');
                    drawPatrialView('../Institution/GetDelete/' + row.data()["Id"], 'lgModalBody');
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

function SetInstitution() {
    var selectedValues = $(this).val();

    if (selectedValues.includes("3")) {
        $.ajax({
            url: '../Institution/GetInstitutions',
            type: 'POST',
            success: function (response) {
                $('#AdditionalSelect').empty();

                $.each(response, function (index, item) {
                    $('#AdditionalSelect').append($('<option>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });
                $('.selectpicker').selectpicker('refresh');
                $('#additionalSelectRow').show();
            }
        });
    } else {
        $('#AdditionalSelect').empty();
        $('#additionalSelectRow').hide();
    }
}

function RoleChangeForInstitution() {
    var selectedValues = $(this).val();

    if (!selectedValues.includes("3")) {
        $('#AdditionalSelect').empty();
        $('#additionalSelectRow').hide();
    }
    else {
        $.ajax({
            url: '../User/PrepareUserInstitution',
            type: 'POST',
            data: { userid: '@Model.Id' }, // Assuming you have the user ID available in your model
            success: function (response) {
                $('#AdditionalSelect').empty();

                $.each(response, function (index, item) {
                    $('#AdditionalSelect').append($('<option>', {
                        value: item.Value,
                        text: item.Text,
                        selected: item.Selected
                    }));
                });

                $('.selectpicker').selectpicker('refresh');
                $('#additionalSelectRow').show();
            },
            error: function (error) {
                console.error('AJAX error:', error);
            }
        });
    }
}

function deleteCurrentInstitution(institutionId) {
    $.ajax({
        url: '../Institution/DeleteConfirmed/' + institutionId,
        cache: false,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.success) {
                //$('#UserDatatable').DataTable().ajax.reload(null, false);
                $('#InstitutionDatatable').DataTable().ajax.reload(null, false);
            }
        }
    });
}

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
            "url": "/User/LoadUserDatatable",
            "type": "POST",
            "dataType": "json",
        },
        "columns": [
            { "data": "Id", "title": "Id", "name": "Id", "visible": false },
            { "data": "UserName", "title": "UserName", "name": "UserName", "autoWidth": true },
            { "data": "Email", "title": "Email", "name": "Email", "autoWidth": true },
            {
                "data": "IsEnabled",
                "title": "IsEnabled",
                "name": "IsEnabled",
                "autoWidth": true,
                "render": function (data, type, row) {
                    if (type === 'display') {
                        return '<input type="checkbox" ' + (data ? 'checked' : '') + ' disabled />';
                    }
                    return data;
                }
            }
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
                    drawPatrialView('../User/GetEdit/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Details':
                    $('#lgModal').modal('show');
                    drawPatrialView('../User/GetDetails/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Delete':
                    $('#lgModal').modal('show');
                    drawPatrialView('../User/GetDelete/' + row.data()["Id"], 'lgModalBody');
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