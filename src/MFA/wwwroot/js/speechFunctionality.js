$(document).ready(function () {

    $('#speech-text-button-trigger').click(function () {
        if (GetTextSpeech() === "") {
            return;
        }
        
        $.ajax({
            type: 'GET',
            url: 'api/speechAnalysisAsync?speechText=' + GetTextSpeech(),

            success: function (fileName) {
                PopulateDiv(fileName);
            },
            error: function (request, status, error) {
                alert(error);
            }
        });
    });
});

function PopulateDiv(fileName) {
    const appendingDiv = "<div id='player'><audio autoplay hidden><source src='audio/" + fileName + ".wav' type='audio/wav'>If you are reading this, audio isn't supported.</audio></div >";
    $('#autoPlayDiv').append(appendingDiv);
}

function GetTextSpeech() {
    var textSpeech = $('#speech-text').val();
    if (textSpeech === undefined) {
        textSpeech = $('#textResults').text();
    }
    return textSpeech;
}