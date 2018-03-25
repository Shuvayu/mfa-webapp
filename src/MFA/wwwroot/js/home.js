$('#uploadedImage').hide();
$('#facialReco').hide();
$('#imageChoosenText').hide();

$(function () {
    $(":file").change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = imageHasBeenLoaded;
            reader.readAsDataURL(this.files[0]);
        }
    });
});

function imageHasBeenLoaded(e) {
    $('#uploadedImage').attr('src', e.target.result);
    $('#uploadedImage').show();
    $('#facialReco').show();
    $('#imageChoosenText').show();
}