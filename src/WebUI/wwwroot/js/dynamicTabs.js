"use strict";

const button = '<button class="close" type="button" title="Remove this page">×</button>';
const paneContent = "HELLOO";

let tabID = 1;

function resetTab() {
    let tabs = $("#tab-list li:not(:first)");

    let len = 1;

    $(tabs).each(function (k, v) {
        len++;
        $(this).find('button').html('Address' + len + button);

    })
    tabID--;
}

$(document).ready(function () {
    $('#btn-add-tab').click(function () {
        console.log("CLICK")
        tabID++;

        $(`#address-tab-${tabID - 1}`).prop('aria-selected', 'false');

        $('#tab-list').append($('<li class="nav-item" role="presentation">' +
            `<button class="nav-link active" id="address-tab-${tabID}" data-bs-toggle="tab" data-bs-target="#address${tabID}" type="button" role="tab" aria-controls="address${tabID}" aria-selected="true">` +
            `<span>Address ${tabID}</span><span class="glyphicon glyphicon-pencil text-muted edit"></span>` +
            ` ${button}` + '</li>'));

        $('#tab-content').append($(`<div class="tab-pane fade" id="address${tabID}" role="tabpanel" aria-labelledby="address-tab-${tabID}">` + paneContent + '</div>'));
        $(".edit").click(editHandler);
    });

    $('#tab-list').on('click', '.close', function () {
        let tabID = $(this).parents('button').attr('id');
        $(this).parents('li').remove();
        $(tabID).remove();

        let firstTab = $('#tab-list button:first');
        resetTab();
        firstTab.Tab('show');
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