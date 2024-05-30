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
    $("#nav-service").hide();
    $("#nav-sla").hide();
    $("#nav-design").hide();
    if (method === "GetSLADocuments") {
        $("#nav-sla").show();
        drawPatrialView('/Bank/GetSLADocuments', '_SLAReports')
    }
    if (method === "GetProjects") {
        $("#nav-design").show();
        drawPatrialView('/Bank/GetProjects', '_Projects')
    }
    if (method === "GetServiceDocuments") {
        $("#nav-service").show();
        drawPatrialView('/Bank/GetServiceDocuments', '_ServiceReports')
    }
    else {
        return;
    }

}