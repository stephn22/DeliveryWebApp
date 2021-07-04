"use strict";

function loadImage(input) {
    $("#spinner").append(`<div id="spinner-div" class="spinner-border text-primary text-start" role="status">
                                        <span class="visually-hidden">Loading...</span>
                                    </div>`);

    setTimeout(readUrl(input), 200);
}

function readUrl(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();

        $("#spinner-div").remove();

        reader.onload = function(i) {
            $("#img-uploaded")
                .attr("src", i.target.result)
                .attr("hidden", false);
        };


        reader.readAsDataURL(input.files[0]);
    }
}

function openFiles() { // open files by clicking the button in the center of image
    document.getElementById("new-img-input").click();
}
