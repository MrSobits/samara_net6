Ext.define('B4.model.edolog.RequestsAppealCits', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'LogRequests'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateActual' },
        { name: 'ActionIntegrationRow', defaultValue: 10 },
        { name: 'NumberGji' }
    ]
});