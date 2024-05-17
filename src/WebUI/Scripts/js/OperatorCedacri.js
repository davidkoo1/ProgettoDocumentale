//InstitutionData
function initializeDocumentDataTable() {
    var table = $('#DocumentDatatable').DataTable({
        //"processing": true,
        "serverSide": true,
        //"scrollX": true,
        //"autoWidth": true,
        //"stateSave": true,
        //"dom": "p", //f, r, t, l, p
        ajax: {
            "url": "/Document/LoadDocumentDatatable",
            "type": "POST",
            "dataType": "json",
        },
        "columns": [
            { "data": "Id", "title": "Id", "name": "Id", "visible": false },
            { "data": "Name", "title": "Name", "name": "Name", "autoWidth": true },
            { "data": "Type", "title": "Type", "name": "Type", "autoWidth": true },
            { "data": "Institution", "title": "Institution", "name": "Institution", "autoWidth": true }
        ],
        columnDefs: [
            { className: 'text-center', targets: [3] },
        ],
    });

    var contextmenu = $('#DocumentDatatable').contextMenu({
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