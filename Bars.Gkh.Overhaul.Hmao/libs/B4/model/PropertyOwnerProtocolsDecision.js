Ext.define('B4.model.PropertyOwnerProtocolsDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PropertyOwnerProtocolsDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Description' },
        { name: 'DocumentFile', defaultValue: null },
        { name: 'Protocol' },
        { name: 'Decision' }
    ]
});