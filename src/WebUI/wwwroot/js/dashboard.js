/********************** CONSTANTS **********************/

const mapboxAccessToken = 'pk.eyJ1Ijoic3RlY3JvdHRpMSIsImEiOiJja3Bna2kzbHYyaThoMm9ueHl1dzlnaTc1In0.EpALSOaDOmuM8XGS_IQzvA';

/**
 * @type {HTMLInputElement}
 */
const newLogo = document.getElementById('new-logo');

/**
 * @type {HTMLParagraphElement}
 */
const countryCodeEl = document.getElementById("country-code");

/**
 * @type {HTMLParagraphElement}
 */
const langCodeEl = document.getElementById("lang-code");

/**
 * @type {HTMLImageElement}
 */
const imgUploaded = document.getElementById('img-uploaded');

/**
 * @type {HTMLInputElement}
 */
const restaurantAddress = document.getElementById('restaurant-address');

/**
 * @type {HTMLSelectElement}
 */
const category = document.getElementById('category');

/**
 * @type {HTMLInputElement}
 */
const restaurantName = document.getElementById('name');

/**
 * @type {HTMLInputElement}
 */
const longitude = document.getElementById('longitude');

/**
 * @type {HTMLInputElement}
 */
const latitude = document.getElementById('latitude');

/**
 * @type {HTMLButtonElement}
 */
const locationBtn = document.getElementById('location-btn');

/**
 * @type {HTMLButtonElement}
 */
const submitBtn = document.getElementById('submit-btn');


/********************** EVENT LISTENERS **********************/

newLogo.addEventListener('change', (input) => {
    if (input.target.files && input.target.files[0]) {
        const reader = new FileReader();

        reader.onload = (e) => {
            imgUploaded.setAttribute("src", e.target.result);
            fadeIn(imgUploaded);
        };

        reader.readAsDataURL(input.target.files[0]);
    }
});

restaurantAddress.addEventListener('input', () => {
    const suggestions = getSuggestions(restaurantAddress.value);

    $("#restaurant-address").autocomplete({
        source: suggestions,
        focus: function (_event, ui) {
            restaurantAddress.value = ui.item.value;
            return false;
        },
        select: function (_event, ui) {
            longitude.value = ui.item.lng;
            latitude.value = ui.item.lat;
        }
    });
});

locationBtn.addEventListener('click', () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition((position) => {
            getCoordinates(position, restaurantAddress);

            window.alert(`latitude: ${latitude.value}, longitude: ${longitude.value}`);
        });
    }
});

/********************** FUNCTIONS **********************/



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

/**
 * Animates an element with fade in transition (0.3s)
 * @param {Element} element element to be animated
 */
 function fadeIn(element) {
    element.removeAttribute("hidden");

    setTimeout(() => {
        element.classList.remove("fade-effect");
    }, 280);
}

/**
 * Animates an element with fade out transition (0.3s)
 * @param {Element} element element to be animated
 */
function fadeOut(element) {
    element.classList.add("fade-effect");
    setTimeout(() => {
        element.setAttribute("hidden", "");
    }, 350);
}

// Enable tooltips

const toolTipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
const tooltipList = toolTipTriggerList.map(function (tooltipTrigger) {
    return new bootstrap.Tooltip(tooltipTrigger);
});