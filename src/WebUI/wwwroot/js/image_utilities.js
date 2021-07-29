function loadImage(input) {
    $("#spinner").append("<div id=\"spinner-div\" class=\"spinner-border text-primary text-start\" role=\"status\">\n                                        <span class=\"visually-hidden\">Loading...</span>\n                                    </div>");
    setTimeout(function () { return readUrl(input); }, 200);
}
function readUrl(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        $("#spinner-div").remove();
        reader.onload = function (i) {
            $("#img-uploaded")
                .attr("src", i.target.result.toString);
            /*.attr("hidden", false)*/
        };
        reader.readAsDataURL(input.files[0]);
    }
}
function openFiles() {
    document.getElementById("new-img-input").click();
}
//# sourceMappingURL=image_utilities.js.map