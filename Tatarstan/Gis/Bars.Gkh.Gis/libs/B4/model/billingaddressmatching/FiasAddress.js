Ext.define('B4.model.billingaddressmatching.FiasAddress', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisAddressMatching',
        listAction: 'GetAdresses'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RegionName' },
        { name: 'CityName' },
        { name: 'StreetName' },
        { name: 'Address' },
        { name: 'Number' },
        { name: 'Letter' },
        { name: 'Housing' },
        { name: 'Building' }
    ]
});
