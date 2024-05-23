﻿//DocumentData
function initializeDocumentDataTable() {
    var table = $('#DocumentDatatable').DataTable({
        //"processing": true,
        "serverSide": true,
        "scrollX": true,
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
            { "data": "TypeId", "title": "Type", "name": "TypeId", "autoWidth": true },
            {
                "data": "GroupingDate",
                "title": "Data",
                "name": "GroupingDate",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return moment(data).format('DD/MM/YYYY'); // Формат даты
                }
            },
            { "data": "InstitutionId", "title": "Institution", "name": "InstitutionId", "autoWidth": true }
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
                    drawPatrialView('../Document/GetEdit/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Details':
                    $('#lgModal').modal('show');
                    drawPatrialView('../Document/GetDetails/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Delete':
                    $('#lgModal').modal('show');
                    drawPatrialView('../Document/GetDelete/' + row.data()["Id"], 'lgModalBody');
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


function DrawDocumentDataTable(resource1, resource2, resource3) {
    var table = $('#DocumentDatatable').DataTable();
    // Непосредственно обновляем данные AJAX-запроса перед отправкой
    var newUrl = '/Document/LoadDocumentDatatable' + '?resource1=' + (resource1 || '') + '&resource2=' + (resource2 || '') + '&resource3=' + (resource3 || '');
    table.ajax.url(newUrl).load();
}

function initializeDocumentThree() {
    $.ajax({
        url: '../Document/GetAllDocumentsThree',
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            buildDocumentList(data);
            attachCaretEventListeners();
        }
    });
}


function buildDocumentList(institutions) {
    var ul = $('#ThreeDocument');
    ul.empty();

    institutions.forEach(function (institution) {
        var caretSpan = $('<span>').addClass('caret');
        var textSpan = $('<span>').text(institution.InstitutionName).css('cursor', 'pointer').on('click', function () {
            DrawDocumentDataTable(institution.InstitutionName, null, null);
        });

        var instituteLi = $('<li>').append(caretSpan, textSpan);
        var yearGroupUl = $('<ul>').addClass('nested');

        institution.YearGroups.forEach(function (yearGroup) {
            var yearCaretSpan = $('<span>').addClass('caret');
            var yearTextSpan = $('<span>').text(yearGroup.Year).css('cursor', 'pointer').on('click', function () {
                DrawDocumentDataTable(institution.InstitutionName, yearGroup.Year, null);
            });

            var yearLi = $('<li>').append(yearCaretSpan, yearTextSpan);
            var typeUl = $('<ul>').addClass('nested');

            yearGroup.SubTypeNames.forEach(function (typeName) {
                var typeLi = $('<li>').text(typeName).css('cursor', 'pointer').on('click', function () {
                    DrawDocumentDataTable(institution.InstitutionName, yearGroup.Year, typeName);
                });
                typeUl.append(typeLi);
            });

            yearLi.append(typeUl);
            yearGroupUl.append(yearLi);
        });

        instituteLi.append(yearGroupUl);
        ul.append(instituteLi);


    });
}



function initializeProjectDataTable() {
    var table = $('#ProjectDatatable').DataTable({
        //"processing": true,
        "serverSide": true,
        "scrollX": true,
        //"autoWidth": true,
        //"stateSave": true,
        //"dom": "p", //f, r, t, l, p
        ajax: {
            "url": "/Project/LoadProjectDatatable",
            "type": "POST",
            "dataType": "json",
        },
        "columns": [
            { "data": "Id", "title": "Id", "name": "Id", "visible": false },
            { "data": "Name", "title": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "DateFrom",
                "title": "DateFrom",
                "name": "DateFrom",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return moment(data).format('DD/MM/YYYY'); // Формат даты
                }
            },
            {
                "data": "DateTill",
                "title": "DateTill",
                "name": "DateTill",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return moment(data).format('DD/MM/YYYY'); // Формат даты
                }
            },
            { "data": "InstitutionId", "title": "Institution", "name": "InstitutionId", "autoWidth": true },
            { "data": "UserId", "title": "User", "name": "UserId", "autoWidth": true },
            {
                "data": "IsActive",
                "title": "IsActive",
                "name": "IsActive",
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
            { className: 'text-center', targets: [2] },
            { className: 'text-center', targets: [3] },
            { className: 'text-center', targets: [6] },
        ],
    });

    var contextmenu = $('#ProjectDatatable').contextMenu({
        selector: 'tr',
        trigger: 'right',
        callback: function (key, options) {
            let row = table.row(options.$trigger);
            switch (key) {
                case 'Edit':
                    $('#lgModal').modal('show');
                    drawPatrialView('../Project/GetUpsert/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Details':
                    $('#lgModal').modal('show');
                    drawPatrialView('../Project/GetDetails/' + row.data()["Id"], 'lgModalBody');
                    break;
                case 'Activate/Disactivate':
                    IsNotActivate(row.data()["Id"]);
                    break;
                default:
                    break
            }
        },
        items: {
            "Edit": { name: "Edit" },
            "Details": { name: "Details" },
            "Activate/Disactivate": { name: "Activate/Disactivate" }
        }
    });

    return table;
}


function IsNotActivate(projectId) {
    $.ajax({
        url: '../Project/Delete/' + projectId,
        cache: false,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.success) {
                $('#ProjectDatatable').DataTable().ajax.reload(null, false);
            }
        }
    });
}
function DrawProjectDataTable(resource1, resource2) {
    var table = $('#ProjectDatatable').DataTable();
    // Непосредственно обновляем данные AJAX-запроса перед отправкой
    var newUrl = '/Project/LoadProjectDatatable' + '?InstitutionId=' + (resource1 || '') + '&YearGroup=' + (resource2 || '');
    table.ajax.url(newUrl).load();
}

function initializeProjectThree() {
    $.ajax({
        url: '../Project/GetAllProjectsThree',
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            buildProjectList(data);    
        }
    });
}

function buildProjectList(institutions) {
    var ul = $('#ThreeProject');
    ul.empty();

    institutions.forEach(function (institution) {
        var caretSpan = $('<span>').addClass('caret');
        var textSpan = $('<span>').text(institution.InstitutionName).css('cursor', 'pointer').on('click', function () {
            DrawProjectDataTable(institution.InstitutionId, null);
        });

        var instituteLi = $('<li>').append(caretSpan, textSpan);
        var yearGroupUl = $('<ul>').addClass('nested');

        institution.YearGroups.forEach(function (yearGroup) {
            var yearTextSpan = $('<span>').text(yearGroup).css('cursor', 'pointer').on('click', function () {
                DrawProjectDataTable(institution.InstitutionId, yearGroup);
            });;
            var yearLi = $('<li>').append(yearTextSpan);
            yearGroupUl.append(yearLi);
        });

        instituteLi.append(yearGroupUl);
        ul.append(instituteLi);


    });

}




function attachCaretEventListeners() {
    var toggler = document.getElementsByClassName("caret");
    for (var i = 0; i < toggler.length; i++) {
        toggler[i].addEventListener("click", function (event) {
            event.stopPropagation(); 
            this.parentElement.querySelector(".nested").classList.toggle("active");
            this.classList.toggle("caret-down");
        });
    }
}

