$('#input-image').on('change', function () {
    var $imgtag = $("#image-tag");
    var file = event.target.files[0];

    var reader = new FileReader();

    $imgtag.title = file.name;

    reader.onload = function (event) {
        $imgtag.attr('src', event.target.result);
    };

    reader.readAsDataURL(file);
});