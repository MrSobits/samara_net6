Ext.define('B4.model.FiasAddress', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'fiasaddress'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AddressName' },
        { name: 'AddressGuid' },
        { name: 'PostCode' },
        { name: 'PlaceCode' },
        { name: 'PlaceGuidId' },
        { name: 'PlaceName' },
        { name: 'PlaceAddressName' },
        { name: 'StreetCode' },
        { name: 'StreetGuidId' },
        { name: 'StreetName' },
        { name: 'House' },
        { name: 'Letter' },
        { name: 'Housing' },
        { name: 'Building' },
        { name: 'Flat' },
        { name: 'Coordinate' }
    ]
});