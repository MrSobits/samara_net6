Ext.define('B4.model.egso.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EgsoIntegrationValues',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EgsoIntegration', defaultValue: null },
        { name: 'MunicipalityDict', defaultValue: null },
        { name: 'Territory', defaultValue: null },
        { name: 'Code', defaultValue: null },
        { name: 'Key', defaultValue: null },
        { name: 'Value', defaultValue: null }
    ]
});