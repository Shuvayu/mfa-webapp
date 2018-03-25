$(document).ready(function () {
    $('#facial-button-trigger').click(function () {
        $.ajax({
            type: 'GET',
            url: 'api/facialAnalysisAsync?Uri=' + GetImageUrl(),

            success: function (msg) {
                PopulateResults(msg, "Face");
            },
            error: function (request, status, error) {
                alert(error);
            }
        });
    });
});

function PopulateResults(value, callType) {
    $('#textResults').text(callType + " in image : " + value);
}
function GetImageUrl() {
    return $('#imageUrl').val();
}
