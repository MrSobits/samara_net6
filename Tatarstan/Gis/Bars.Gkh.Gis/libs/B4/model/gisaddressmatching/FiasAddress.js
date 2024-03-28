Ext.define('B4.model.gisaddressmatching.FiasAddress', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisAddressMatching',
        listAction: 'GetFiasAdresses'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RegionName' },
        { name: 'CityName' },
        { name: 'StreetName' },
        { name: 'Address' },
        { name: 'Number' }
    ]
});
