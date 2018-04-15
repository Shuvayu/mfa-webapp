$(document).ready(function () {
    
    $("#workflow-button-trigger").click(function () {
        $.ajax({
            type: 'GET',
            url: 'api/workFlowAsync?Uri=' + GetImageUrl(),

            success: function (msg) {
                PopulateResults(msg, "Workflow");
            },
            error: function (request, status, error) {
                alert(error);
            }
        });
    });
});

function PopulateResults(value, callType) {
    $('#textResults').text(callType + " : " + value);
}

function GetImageUrl() {
    return $('#imageUrl').val();
}