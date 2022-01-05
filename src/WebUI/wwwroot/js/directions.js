mapboxgl.accessToken = 'pk.eyJ1Ijoic3RlY3JvdHRpMSIsImEiOiJja3Bna2kzbHYyaThoMm9ueHl1dzlnaTc1In0.EpALSOaDOmuM8XGS_IQzvA';
mapboxgl.setRTLTextPlugin('https://api.mapbox.com/mapbox-gl-js/plugins/mapbox-gl-rtl-text/v0.2.3/mapbox-gl-rtl-text.js');

/**
 * 
 * @param {number} id 
 * @param {string} placeName 
 * @param {number} longitude 
 * @param {number} latitude 
 * @param {number} customerId 
 * @param {number} restaurateurId 
 */
function Address(id, placeName, longitude, latitude, customerId, restaurateurId) {
    this.id = id;
    this.placeName = placeName;
    this.longitude = longitude;
    this.latitude = latitude;
    this.customerId = customerId;
    this.restaurateurId = restaurateurId;
}

/**
 * @type {HTMLButtonElement}
 */
const startBtn = document.getElementById('start');
const success = document.getElementById("success");
const failed = document.getElementById("failed");

const restaurantAddress = document.getElementById("restaurant-address");

const deliveryAddress = document.getElementById("delivery-address");

const addressId = parseInt(restaurantAddress.getAttribute("data-id"));
const deliveryAddressId = parseInt(deliveryAddress.getAttribute("data-id"));

/**
 * @type {Address}
 */
let originAddress;

let start = [0, 0];
let end = [0, 0];

/**
 * @type {Address}
 */
let destinationAddress;

// get origin address
fetch(`/api/addresses/${addressId}`, {
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
}).then((res) => {
    if (res.status === 200) {
        res.json().then((data) => {
            originAddress = new Address(
                data.id,
                data.placeName,
                data.longitude,
                data.latitude,
                data.customerId,
                data.restaurateurId
            );

            start = [originAddress.longitude, originAddress.latitude];
        });
    }
});

// initialize map
const map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/streets-v10',
    center: start, // starting position
    zoom: 12
});

// get destination address
fetch(`/api/addresses/${deliveryAddressId}`, {
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
}).then((res) => {
    if (res.status === 200) {
        res.json().then((data) => {
            destinationAddress = new Address(
                data.id,
                data.placeName,
                data.longitude,
                data.latitude,
                data.customerId,
                data.restaurateurId
            );

            end = [destinationAddress.longitude, destinationAddress.latitude];
        });
    }
});

startBtn.addEventListener('click', () => {
    disableBtn(startBtn);
    enableBtn(success);
    enableBtn(failed);

    getRoute();

    map.flyTo({
        center: start,
        zoom: 15
    });
});

// initialize the map canvas to interact with later
const canvas = map.getCanvasContainer();

function getRoute() {
    const url = `https://api.mapbox.com/directions/v5/mapbox/driving/${start[0]},${start[1]};${end[0]},${end[1]}?steps=true&geometries=geojson&access_token=${mapboxgl.accessToken}`;

    fetch(url, {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }).then((res) => {
        if (res.status === 200) {
            res.json().then((data) => {
                const route = data.route[0].geometry.coordinates;

                const geojson = {
                    type: "Feature",
                    properties: {},
                    geometry: {
                        type: "LineString",
                        coordinates: route
                    }
                };

                // get the sidebar and add the instructions
                const instructions = document.getElementById("instructions");
                const steps = data.routes[0].legs[0].steps;

                const tripInstructions = [];

                for (let i = 0; i < steps.length; i++) {
                    tripInstructions.push(`<br /><li>${steps[i].maneuver.instruction}</li>`);
                    instructions.innerHTML = `<br><span class="duration">Trip duration: ${Math.floor(data.duration / 60)} min </span>${tripInstructions}`;
                }

                // if the route already exists on the map, reset it using setData
                if (map.getSource("route")) {
                    map.getSource("route").setData(geojson);
                } else { // otherwise, make a new request
                    map.addLayer({
                        id: 'route',
                        type: 'line',
                        source: {
                            type: 'geojson',
                            data: {
                                type: 'Feature',
                                properties: {},
                                geometry: {
                                    type: 'LineString',
                                    coordinates: geojson
                                }
                            }
                        },
                        layout: {
                            'line-join': 'round',
                            'line-cap': 'round'
                        },
                        paint: {
                            'line-color': '#3887be',
                            'line-width': 5,
                            'line-opacity': 0.75
                        }
                    });
                }

            });
        }
    });
}

map.on('load', function () {
    // make an initial directions request that
    // starts and ends at the same location
    map.addControl(new mapboxgl.FullscreenControl());

    // Add geolocate control to the map.
    map.addControl(
        new mapboxgl.GeolocateControl({
            positionOptions: {
                enableHighAccuracy: true
            },
            // When active the map will receive updates to the device's location as it changes.
            trackUserLocation: true,
            // Draw an arrow next to the location dot to indicate which direction the device is heading.
            showUserHeading: true
        })
    );

    // Add starting point to the map
    map.addLayer({
        id: 'point',
        type: 'circle',
        source: {
            type: 'geojson',
            data: {
                type: 'FeatureCollection',
                features: [
                    {
                        type: 'Feature',
                        properties: {},
                        geometry: {
                            type: 'Point',
                            coordinates: start
                        }
                    }
                ]
            }
        },
        paint: {
            'circle-radius': 10,
            'circle-color': '#3887be'
        }
    });

    // Add end point to the map
    map.addLayer({
        id: 'end',
        type: 'circle',
        source: {
            type: 'geojson',
            data: {
                type: 'FeatureCollection',
                features: [
                    {
                        type: 'Feature',
                        properties: {},
                        geometry: {
                            type: 'Point',
                            coordinates: end
                        }
                    }
                ]
            }
        },
        paint: {
            'circle-radius': 10,
            'circle-color': '#f30'
        }
    });
});

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