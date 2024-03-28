Ext.define('B4.model.service.Housing', {
    extend: 'B4.model.service.Base',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HousingService'
    },
    fields: [
        { name: 'ProtocolNumber' },
        { name: 'ProtocolFrom', defaultValue: null },
        { name: 'Equipment', defaultValue: 10 },
        { name: 'Periodicity', defaultValue: null },
        { name: 'TypeOfProvisionService', defaultValue: 10 },
        { name: 'PricePurchasedResources', defaultValue: null },
        { name: 'Protocol', defaultValue: null }
    ]
});