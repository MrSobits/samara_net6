Ext.define('B4.model.administration.ExportableType', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'Description' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'DataTransferIntegration',
        listAction: 'GetExportableTypes'
    }
});