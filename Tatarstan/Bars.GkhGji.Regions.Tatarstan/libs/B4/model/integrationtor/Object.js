Ext.define('B4.model.integrationtor.Object', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TorIntegration',
        listAction: 'GetObjects'
    },
    fields: [
        { name: 'TorId' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'FiasAddress' }
    ]
});