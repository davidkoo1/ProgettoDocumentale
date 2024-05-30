function HideAndShow(element) {
    $(element).next(".report-list").toggle();
}

function toggleNested(element) {
    $(element).next(".nested").toggle();
    $(element).toggleClass("caret-down");
}

function loadReportDetails(reportId) {
    $.ajax({
        url: '../Bank/GetDetails',
        data: { id: reportId },
        success: function (data) {
            $("#downloadDocument").attr("href", `../Bank/DownloadDocumentById/${reportId}`);
            $("#CardDetails").show();
            $("#Details").html(null);
            $("#Details").html(data);
        }
    });
}


function drawReports(method) {
    if (method === "GetSLADocuments") {
        drawPatrialView('/Bank/GetSLADocuments', '_Report')
    }
    if (method === "GetProjects") {
        drawPatrialView('/Bank/GetProjects', '_Report')
    }
    if (method === "GetServiceDocuments") {
        drawPatrialView('/Bank/GetServiceDocuments', '_Report')
    }
    else {
        return;
    }

}