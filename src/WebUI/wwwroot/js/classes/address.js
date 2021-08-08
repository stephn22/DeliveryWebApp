class Address {
    constructor(addressLine1, addressLine2, number, city, postalCode, country, stateProvince, longitude, latitude) {
        this.addressLine1 = addressLine1;
        this.addressLine2 = addressLine2;
        this.number = number;
        this.city = city;
        this.postalCode = postalCode;
        this.country = country;
        this.stateProvince = stateProvince;
        this.longitude = longitude;
        this.latitude = latitude;
    }

    //constructor(json) {
    //    this.addressLine1 = json.addressLine1;
    //    this.addressLine2 = json.addressLine2;
    //    this.number = json.number;
    //    this.city = json.city;
    //    this.postalCode = json.postalCode;
    //    this.country = json.country;
    //    this.stateProvince = json.stateProvince;
    //    this.longitude = json.longitude;
    //    this.latitude = json.latitude;
    //}
}