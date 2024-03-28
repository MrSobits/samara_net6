Ext.define('B4.model.ServiceSettings', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceSettings',
        listAction: 'GetRegisteredSettings'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'IntegrationService' },
        { name: 'ServiceAddress' },
        { name: 'AsyncServiceAddress' }
    ]
});
