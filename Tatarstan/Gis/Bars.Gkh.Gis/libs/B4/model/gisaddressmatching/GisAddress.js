Ext.define('B4.model.gisaddressmatching.GisAddress', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisAddressMatching',
        listAction: 'GetAddresses'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Country', defaulValue: null },
        { name: 'Region', defaulValue: null },
        { name: 'City', defaulValue: null },
        { name: 'Street', defaulValue: null },
        { name: 'CountryName' },
        { name: 'RegionName' },
        { name: 'CityName' },
        { name: 'StreetName' },
        { name: 'Address' },
        { name: 'Number' },
        { name: 'Supplier' },
        { name: 'TypeAddressMatched', defaultValue: 10 },
        { name: 'HouseType' },
        { name: 'Municipality' }
    ]
});
