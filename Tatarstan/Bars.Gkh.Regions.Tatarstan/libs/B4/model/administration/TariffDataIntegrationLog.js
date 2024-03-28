Ext.define('B4.model.administration.TariffDataIntegrationLog', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TariffDataIntegrationLog'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TariffDataIntegrationMethod' },
        { name: 'Login' },
        { name: 'StartMethodTime' },
        { name: 'Parameters' },
        { name: 'ExecutionStatus' },
        { name: 'LogFileId' }
    ]
});