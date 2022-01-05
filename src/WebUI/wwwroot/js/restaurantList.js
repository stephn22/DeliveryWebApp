mapboxgl.accessToken = 'pk.eyJ1Ijoic3RlY3JvdHRpMSIsImEiOiJja3Bna2kzbHYyaThoMm9ueHl1dzlnaTc1In0.EpALSOaDOmuM8XGS_IQzvA';

const range = document.getElementById("range");
const output = document.getElementById("range-output");

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
 * 
 * @param {number} id 
 * @param {string} name 
 * @param {Blob} logo 
 * @param {number} addressId 
 * @param {Address} address 
 * @param {string} category 
 * @param {number} customerId 
 */
function Restaurateur(id, name, logo, addressId, address, category, customerId) {
    this.id = id;
    this.name = name;
    this.logo = logo;
    this.addressId = addressId;
    this.address = address;
    this.category = category;
    this.customerId = customerId;
}

if (!('remove' in Element.prototype)) {
    Element.prototype.remove = function () {
        if (this.parentNode) {
            this.parentNode.removeChild(this);
        }
    };
}


output.innerHTML = `Range (Km): ${range.value}`;

range.addEventListener("input", () => {
    output.innerHTML = `Range (Km): ${this.value}`;
});


let map;
let point;

/**
 * @type {Restaurateur[]}
 */
const restaurateurs = [];

const stores = {
    "type": "FeatureCollection",
    "features": []
};

fetch("/api/restaurateurs", {
    method: "GET",
    headers: {
        "Content-Type": "application/json"
    }
}).then((res) => {
    if (res.status === 200) {
        res.json().then((data) => {

            data.forEach((item) => {

                const restaurateur = new Restaurateur(
                    item.id,
                    item.restaurantName,
                    item.logo,
                    item.restaurantAddressId,
                    null,
                    item.category,
                    item.customerId);

                fetch(`/api/addresses/${restaurateur.addressId}`, {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json"
                    }
                }).then((res) => {
                    if (res.status == 200) {
                        res.json().then((addressData) => {

                            const address = new Address(
                                addressData.id,
                                addressData.placeName,
                                addressData.longitude,
                                addressData.latitude,
                                addressData.customerId,
                                addressData.restaurateurId);

                            restaurateur.address = address;

                            restaurateurs.push(restaurateur);
                        });
                    }
                });
            });
        });
    }
});

restaurateurs.forEach((restaurateur) => {
    stores.features.push({
        type: "Feature",
        geometry: {
            type: "Point",
            coordinates: [restaurateur.address.longitude, restaurateur.address.latitude]
        },
        properties: {
            // TODO: logo,
            restaurateurId: restaurateur.id,
            name: restaurateur.name,
            category: restaurateur.category,
            address: restaurateur.address.placeName,
        }
    });
});

/**
 * Assign a unique id to each store. This `id` is used
 * later to associate each point on the map with a listing
 * in the sidebar.
*/
stores.features.forEach(function (store, i) {
    value: store.properties.id = i;
});

function showMap(center) {
    map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [center.coords.longitude, center.coords.latitude],
        zoom: 12,
        scrollZoom: true
    });

    map.on('load',
        function (e) {
            map.addSource('places',
                {
                    'type': 'geojson',
                    'data': stores
                });
            point = {
                "type": "FeatureCollection",
                "features": [
                    {
                        "type": "Feature",
                        "geometry": {
                            "type": "Point",
                            "coordinates": [center.coords.longitude, center.coords.latitude]
                        }
                    }
                ]
            };

            buildLocationList(stores);
            addMarkers();
        });
}

range.addEventListener("mouseup", function (e) {
    var options = { units: "kilometers" };

    /**
     * Loop to iterate through all the store locations,
     * define a new property for each object called distance, and set the value
     * of that property to the distance
     * between the coordinates stored in the searchResult and the coordinates
     * of each store location
     */
    stores.features.forEach(function (store) {
        Object.defineProperty(store.properties,
            "distance",
            {
                value: turf.distance(store.geometry, point.features[0].geometry, options),
                writable: true,
                enumerable: true,
                configurable: true
            });
    });

    /**
     * Sort the objects in the stores array
     * by the distance property added earlier
     */
    stores.features.sort(function (a, b) {
        return Math.abs(range.value - a.properties.distance) - Math.abs(range.value - b.properties.distance);
    });

    /*
     * Remove the current list of stores and
     * rebuild the list using the reordered array created
     */
    const listings = document.getElementById("listings");

    while (listings.firstChild) {
        listings.removeChild(listings.firstChild);
    }

    buildLocationList(stores);

    const activeListing = document.getElementById(`listing-${0}`);
    activeListing.classList.add("active");

    var bbox = getBbox(stores, 0, point.features[0].geometry);

    map.fitBounds(bbox, {
        padding: 100
    });

    createPopUp(stores.features[0]);
});

function addMarkers() {
    /* For each feature in the GeoJSON object above: */
    stores.features.forEach(function (marker) {

        /* Create a div element for the marker. */
        const el = document.createElement("div");

        /* Assign a unique `id` to the marker. */
        el.id = `marker-${marker.properties.id}`;

        /* Assign the `marker` class to each marker for styling. */
        el.className = "marker";

        /**
         * Create a marker using the div element
         * defined above and add it to the map.
         **/
        new mapboxgl.Marker(el, { offset: [0, -23] })
            .setLngLat(marker.geometry.coordinates)
            .addTo(map);

        /**
         * Listen to the element and when it is clicked, do three things:
         * 1. Fly to the point
         * 2. Close all other popups and display popup for clicked store
         * 3. Highlight listing in sidebar (and remove highlight for all other listings)
         **/
        el.addEventListener("click", function (e) {
            /* Fly to the point */
            flyToStore(marker);

            /* Close all other popups and display popup for clicked store */
            createPopUp(marker);

            /* Highlight listing in sidebar */
            const activeItem = document.getElementsByClassName("active");

            e.stopPropagation();

            if (activeItem[0]) {
                activeItem[0].classList.remove("active");
            }

            const listing = document.getElementById(`listing-${marker.properties.id}`);
            listing.classList.add("active");
        });
    });
}

/**
 * Add a listing for each store to the sidebar.
**/
function buildLocationList(data) {
    data.features.forEach(function (store, i) {

        const prop = store.properties;
        /* Add a new listing section to the sidebar. */

        const listings = document.getElementById("listings");
        const listing = listings.appendChild(document.createElement("div"));

        /* Assign a unique `id` to the listing. */
        listing.id = `listing-${i}`;

        /* Assign the `item` class to each listing for styling. */
        listing.className = "item";

        /* Add the link to the individual listing created above. */
        const link = listing.appendChild(document.createElement("a"));

        link.href = "#";
        link.className = "title";
        link.id = `link-${i}`;
        link.innerHTML = prop.name;

        /* Add details to the individual listing. */
        const details = listing.appendChild(document.createElement("div"));

        if (prop.category) {
            details.innerHTML += prop.category;
        }

        if (prop.distance) {
            const roundedDistance = Math.round(prop.distance * 100) / 100;
            details.innerHTML += `<p><strong></p>${roundedDistance} @localizer["kilometers away"]</strong></p>`;
        }

        // initialize range value with the first store distance in the list
        //range.value = stores.features[0].properties.distance;
        //output.innerHTML = `Range (Km): ${range.value}`;
        /**
         * Listen to the element and when it is clicked, do four things:
         * 1. Update the `currentFeature` to the store associated with the clicked link
         * 2. Fly to the point
         * 3. Close all other popups and display popup for clicked store
         * 4. Highlight listing in sidebar (and remove highlight for all other listings)
        **/
        link.addEventListener("click", function (e) {
            for (let i = 0; i < data.features.length; i++) {
                if (this.id === `link-${i}`) {
                    const clickedListing = data.features[i];

                    flyToStore(clickedListing);
                    createPopUp(clickedListing);
                }
            }

            const activeItem = document.getElementsByClassName("active");

            if (activeItem[0]) {
                activeItem[0].classList.remove('active');
            }

            this.parentNode.classList.add('active');
        });
    });
}

/**
 * Use Mapbox GL JS's `flyTo` to move the camera smoothly
 * a given center point.
**/
function flyToStore(currentFeature) {
    map.flyTo({
        center: currentFeature.geometry.coordinates,
        zoom: 11
    });
}

/**
 * Create a Mapbox GL JS `Popup`.
**/
function createPopUp(currentFeature) {
    const popUps = document.getElementsByClassName('mapboxgl-popup');

    if (popUps[0]) popUps[0].remove();

    const popup = new mapboxgl.Popup({ closeOnClick: false })
        .setLngLat(currentFeature.geometry.coordinates)
        .setHTML(
            `<h3>${currentFeature.properties.name}</h3><h4>${currentFeature.properties.address}</h4><a href="RestaurantDetail/${currentFeature.properties.restaurateurId}">@localizer["Go to vendor page"]</a>`
        )
        .addTo(map);
}

function getBbox(sortedStores, storeIdentifier, searchResult) {
    const lats = [
        sortedStores.features[storeIdentifier].geometry.coordinates[1], searchResult.coordinates[1]
    ];

    const lons = [
        sortedStores.features[storeIdentifier].geometry.coordinates[0], searchResult.coordinates[0]
    ];

    const sortedLons = lons.sort(function (a, b) {
        if (a > b) {
            return 1;
        }
        if (a.distance < b.distance) {
            return -1;
        }
        return 0;
    });

    const sortedLats = lats.sort(function (a, b) {
        if (a > b) {
            return 1;
        }
        if (a.distance < b.distance) {
            return -1;
        }
        return 0;
    });

    return [[sortedLons[0], sortedLats[0]], [sortedLons[1], sortedLats[1]]];
}
