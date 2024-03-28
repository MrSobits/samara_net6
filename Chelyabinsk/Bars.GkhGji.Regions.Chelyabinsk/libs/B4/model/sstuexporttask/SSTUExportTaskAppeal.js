Ext.define('B4.model.sstuexporttask.SSTUExportTaskAppeal', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SSTUExportTaskAppeal'
    },
    fields: [
        { name: 'Id' },
        { name: 'SSTUExportTask' },
        { name: 'AppealCits' },
        { name: 'AppealCitsDate' }
    ]
});