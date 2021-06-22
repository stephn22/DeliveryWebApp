"use strict";

const button = '<button class="btn-close" id="close-btn" aria-label="Close" type="button" title="Remove this page"></button>';

let tabID = 1;

function resetTab() {
    const tabs = $("#tab-list li:not(:first)");

    let len = 1;

    $(tabs).each(function (k, v) {
        len++;
        $(this).find("button").html(`Address${len}${button}`);

    })
    tabID--;
}

$(document).ready(function () {
    $("#btn-add-tab").click(function () {
        tabID++;

        $(`#address-tab-${tabID - 1}`).prop("aria-selected", "false");
        $(`#address-tab-${tabID}`).prop("aria-selected", "active");

        $("#tab-list").append($(`<li class="nav-item" role="presentation">${`<button class="nav-link active" id="address-tab-${tabID}"
            data-bs-toggle="tab" data-bs-target="#address${tabID}" type="button" role="tab" aria-controls="address${tabID}" aria-selected="true">`}${`Address ${tabID}
            `}${` ${button}`}</li>`));

        $("#tab-content").append($(`<div class="tab-pane fade show active" id="address${tabID}" role="tabpanel" aria-labelledby="address-tab-${tabID}">` + "</div>"));
        $(".edit").click(editHandler);
    });

    $("#tab-list").on("click", "#close-btn", function () {
        const tabID = $(this).parents("button").attr("id");
        $(this).parents("li").remove();
        $(tabID).remove();

        const firstTab = $("#tab-list button:first");
        resetTab();
        firstTab.show();
    });

    let list = document.getElementById("tab-list");
});

const editHandler = function () {
    const t = $(this);
    t.css("visibility", "hidden");
    $(this).prev().attr("contenteditable", "true").focusout(function () {
        $(this).removeAttr("contenteditable").off("focusout");
        t.css("visibility", "visible");
    });
};