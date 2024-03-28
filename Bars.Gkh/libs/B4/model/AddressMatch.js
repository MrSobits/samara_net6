Ext.define('B4.model.AddressMatch', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AddressMatch'
    },
    fields: [
        { name: 'Id', defaultValue: 0 },
        { name: 'ExternalAddress' },
        { name: 'RealityObject' }
    ]
});