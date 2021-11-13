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

/********************** EVENT LISTENERS **********************/

locationFirstBtn.addEventListener("click", () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition((position) => {
            getCoordinates(position, inputFirstAddress);
        });
    }
});

locationSecondBtn.addEventListener("click", () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition((position) => {
            getCoordinates(position, inputSecondAddress);
        });
    }
});

inputFirstAddress.addEventListener("input", () => {
    const suggestions = getSuggestions(inputFirstAddress.value);
    $("#input-first-address").autocomplete({
        source: suggestions
    });
});

inputSecondAddress.addEventListener("input", () => {
    const suggestions = getSuggestions(inputSecondAddress.value);
    $("#input-second-address").autocomplete({
        source: suggestions
    });
});

saveFirstAddressBtn.addEventListener("click", () => {
    const address = {
        customerId: customerIdEl.getAttribute("data-id"),
        address: inputFirstAddress.value,
        lat: inputFirstAddress.getAttribute("data-lat"),
        lng: inputFirstAddress.getAttribute("data-lng")
    };
    uploadAddress(address);
});

saveSecondAddressBtn.addEventListener("click", () => {
    const address = {
        customerId: customerIdEl.getAttribute("data-id"),
        address: inputSecondAddress.value,
        lat: inputSecondAddress.getAttribute("data-lat"),
        lng: inputSecondAddress.getAttribute("data-lng")
    };
    uploadAddress(address);
});

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
 * @param {{customerId: number, address: string, lat: number, lng: number}} address address object
 */
function uploadAddress(address) {
    fetch("api/addresses", // FIXME: 404
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(address)
        }).then((res) => {
            if (res.status === 200) {
                console.log("Address uploaded");
            } else {
                window.alert(`Error: ${res.status}`);
            }
        }).catch((error) => window.alert(error));
}

/**
 * Retrieves suggestions for a searchstring from Mapbox API
 * @param {string} searchString 
 * @returns {string[]} array of suggestions
 */
function getSuggestions(searchString) {
    const uuid = getUUID();
    const countryCode = countryCodeEl.getAttribute("data-country");
    const langCode = langCodeEl.getAttribute("data-lang");

    /**
     * @type {string[]}
    */
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
                        suggestions.push(feature.place_name);
                    });
                });
            }
        }).catch((error) => window.alert(error));

    return suggestions;
}

/**
 * Generates a random UUID in the UUIDv4 format
 * @returns {string} UUID generated
 */
function getUUID() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

// Enable tooltips

const toolTipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
const tooltipList = toolTipTriggerList.map(function (tooltipTrigger) {
    return new bootstrap.Tooltip(tooltipTrigger);
});
