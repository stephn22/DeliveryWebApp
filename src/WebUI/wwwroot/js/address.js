/********************** CONSTANTS **********************/

const mapboxAccessToken = 'pk.eyJ1Ijoic3RlY3JvdHRpMSIsImEiOiJja3Bna2kzbHYyaThoMm9ueHl1dzlnaTc1In0.EpALSOaDOmuM8XGS_IQzvA';

/**
 * @type {HTMLButtonElement}
 */
const locationFirstBtn = document.getElementById("location-first-address-btn");

/**
 * @type {HTMLButtonElement}
 */
const locationSecondBtn = document.getElementById("location-second-address-btn");

/**
 * @type {HTMLInputElement}
 */
const inputFirstAddress = document.getElementById("input-first-address");

/**
 * @type {HTMLInputElement}
 */
const inputSecondAddress = document.getElementById("input-second-address");

/**
 * @type {HTMLDivElement}
 */
const firstAddressForm = document.getElementById("first-address-form");

/**
 * @type {HTMLDivElement}
 */
const secondAddressForm = document.getElementById("second-address-form");

/**
 * @type {HTMLParagraphElement}
 */
const customerIdEl = document.getElementById("customer-id");

/**
 * @type {HTMLParagraphElement}
 */
const countryCodeEl = document.getElementById("country-code");

/**
 * @type {HTMLParagraphElement}
 */
const langCodeEl = document.getElementById("lang-code");

/**
 * @type {HTMLButtonElement}
 */
const saveFirstAddressBtn = document.getElementById("save-first-address-btn");

/**
 * @type {HTMLButtonElement}
 */
const saveSecondAddressBtn = document.getElementById("save-second-address-btn");

/**
 * @type {HTMLFormElement}
 */
const firstForm = document.getElementById("first-form");

/**
 * @type {HTMLFormElement}
 */
const secondForm = document.getElementById("second-form");

/**
 * @type {HTMLButtonElement}
 */
const deleteFirstAddressBtn = document.getElementById("delete-first-address-btn");

/**
 * @type {HTMLButtonElement}
 */
const deleteSecondAddressBtn = document.getElementById("delete-second-address-btn");

/********************** EVENT LISTENERS **********************/

locationFirstBtn.addEventListener("click", () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition((position) => {
            getCoordinates(position, inputFirstAddress);
            enableBtn(saveFirstAddressBtn);
        });
    }
});

locationSecondBtn.addEventListener("click", () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition((position) => {
            getCoordinates(position, inputSecondAddress);
            enableBtn(saveSecondAddressBtn);
        });
    }
});

inputFirstAddress.addEventListener("input", () => {
    if (inputFirstAddress.value.length > 0) {
        enableBtn(saveFirstAddressBtn);

        const suggestions = getSuggestions(inputFirstAddress.value);

        $("#input-first-address").autocomplete({
            source: suggestions,
            focus: function (_event, ui) {
                inputFirstAddress.value = ui.item.value;
                return false;
            },
            select: function (_event, ui) {
                inputFirstAddress.setAttribute("data-lat", ui.item.lat);
                inputFirstAddress.setAttribute("data-lng", ui.item.lng);
            }
        });
    } else {
        disableBtn(saveFirstAddressBtn);
    }
});

inputSecondAddress.addEventListener("input", () => {
    if (inputSecondAddress.value.length > 0) {
        enableBtn(saveSecondAddressBtn);

        const suggestions = getSuggestions(inputSecondAddress.value);

        $("#input-second-address").autocomplete({
            source: suggestions,
            focus: function (_event, ui) {
                inputSecondAddress.value = ui.item.value;
                return false;
            },
            select: function (_event, ui) {
                inputSecondAddress.setAttribute("data-lat", ui.item.lat);
                inputSecondAddress.setAttribute("data-lng", ui.item.lng);
            }
        });
    } else {
        disableBtn(saveSecondAddressBtn);
    }
});

if (saveFirstAddressBtn) {
    disableBtn(saveFirstAddressBtn);

    saveFirstAddressBtn.addEventListener("click", () => {
        const address = {
            id: null,
            customer: null,
            customerId: customerIdEl.getAttribute("data-id"),
            restaurateurId: null,
            restaurateur: null,
            placeName: inputFirstAddress.value,
            latitude: inputFirstAddress.getAttribute("data-lat"),
            longitude: inputFirstAddress.getAttribute("data-lng")
        };
        uploadAddress(address);

    });
}

if (saveSecondAddressBtn) {
    disableBtn(saveSecondAddressBtn);

    saveSecondAddressBtn.addEventListener("click", () => {
        const address = {
            id: null,
            customerId: customerIdEl.getAttribute("data-id"),
            customer: null,
            restaurateurId: null,
            restaurateur: null,
            placeName: inputSecondAddress.value,
            latitude: inputSecondAddress.getAttribute("data-lat"),
            longitude: inputSecondAddress.getAttribute("data-lng")
        };
        uploadAddress(address);
    });
}

if (deleteFirstAddressBtn) {
    deleteFirstAddressBtn.addEventListener("click", () => {
        const id = parseInt(inputFirstAddress.getAttribute("data-id"));

        deleteAddress(id, inputFirstAddress, deleteFirstAddressBtn, locationFirstBtn, saveFirstAddressBtn);
    });
}

if (deleteSecondAddressBtn) {
    deleteSecondAddressBtn.addEventListener("click", () => {
        const id = parseInt(inputSecondAddress.getAttribute("data-id"));

        deleteAddress(id, inputSecondAddress, deleteSecondAddressBtn, locationSecondBtn, saveSecondAddressBtn);
    });
}

/********************** FUNCTIONS **********************/

/**
 * Perform a reverse geocode request to get the address from the coordinates
 * @param {GeolocationPosition} position position object
 * @param {HTMLInputElement} addressInputElement input element to set the address properties
 */
function getCoordinates(position, addressInputElement) {
    fetch(`https://api.mapbox.com/geocoding/v5/mapbox.places/${position.coords.longitude},${position.coords.latitude}.json?limit=1&access_token=${mapboxAccessToken}`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then((res) => {
            if (res.status === 200) {
                res.json().then((data) => { // forward geocoding success
                    addressInputElement.value = data.features[0].place_name;
                    addressInputElement.setAttribute("data-lat", position.coords.latitude);
                    addressInputElement.setAttribute("data-lng", position.coords.longitude);
                });

            }
        }).catch((error) => window.alert(error));
}

/**
 * Upload the address to the database
 * @param {*} address address object
 */
function uploadAddress(address) {
    fetch("/api/addresses",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(address)
        }).then((res) => {
            if (res.status === 200) {
                console.log("Address uploaded");

                window.location.reload();
            } else {
                window.alert(`Error: ${res.status}`);
            }
        }).catch((error) => window.alert(error));
}

/**
 * Delete the address with id from the database, then show the location button, save button and hide the delete button
 * @param {number} id id of the address to delete
 * @param {HTMLInputElement} inputElement input element to emtpy
 * @param {HTMLButtonElement} deleteBtn button to hide
 * @param {HTMLButtonElement} locationBtn button to show
 * @param {HTMLButtonElement} saveBtn button to show
 */
function deleteAddress(id, inputElement, deleteBtn, locationBtn, saveBtn) {
    fetch(`/api/addresses/${id}`, {
        method: 'DELETE',
    }).then((res) => {
        if (res.status === 200) {
            console.log("Address deleted");

            // empty the input
            inputElement.value = "";
            inputElement.removeAttribute("disabled");

            // hides delete button
            hideItem(deleteBtn);

            // shows location button
            showItem(locationBtn);

            // shows save button
            showItem(saveBtn);
            disableBtn(saveBtn);
        } else {
            window.alert(`Error: ${res.status}`);
        }
    }).catch((error) => window.alert(error));
}

/**
 * Retrieves suggestions for a searchstring from Mapbox API
 * @param {string} searchString 
 * @returns {*[]} array of suggestions
 */
function getSuggestions(searchString) {
    const countryCode = countryCodeEl.getAttribute("data-country");
    const langCode = langCodeEl.getAttribute("data-lang");

    let suggestions = [];

    fetch(`https://api.mapbox.com/geocoding/v5/mapbox.places/${searchString}.json?autocomplete=true&types=address&country=${countryCode}&language=${langCode}&limit=5&access_token=${mapboxAccessToken}`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then((res) => {
            if (res.status === 200) {

                res.json().then((data) => { // retrieving suggestions success
                    data.features.forEach((feature) => {
                        suggestions.push({
                            value: feature.place_name,
                            lat: feature.center[1],
                            lng: feature.center[0]
                        });
                    });
                });
            }
        }).catch((error) => window.alert(error));

    return suggestions;
}

/**
 * Hides an item
 * @param {HTMLElement} item item to be hidden
 */
function hideItem(item) {
    item.setAttribute("hidden", "");
}

/**
 * Shows an item
 * @param {HTMLElement} item item to be shown
 */
function showItem(item) {
    item.removeAttribute("hidden");
}

/**
 * Enables a button
 * @param {HTMLButtonElement} btn button to be enabled
 */
function enableBtn(btn) {
    btn.removeAttribute("disabled");
}

/**
 * Disables a button
 * @param {HTMLButtonElement} btn button to be disabled
 */
function disableBtn(btn) {
    btn.setAttribute("disabled", "disabled");
}

// Enable tooltips

const toolTipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
const tooltipList = toolTipTriggerList.map(function (tooltipTrigger) {
    return new bootstrap.Tooltip(tooltipTrigger);
});
