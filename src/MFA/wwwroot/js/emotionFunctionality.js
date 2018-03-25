$(document).ready(function () {
    
    $("#emotion-button-trigger").click(function () {
        $.ajax({
            type: 'GET',
            url: 'api/emotionalAnalysisAsync?Uri=' + GetImageUrl(),

            success: function (msg) {
                PopulateResults(msg, "Emotion");
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