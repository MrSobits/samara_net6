Ext.define('B4.model.sstuexporttask.SSTUExportTask', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SSTUExportTask'
    },
    fields: [
        { name: 'Id' },
        { name: 'Operator' },
        { name: 'SSTUExportState' },
        { name: 'SSTUSource' },
        { name: 'TaskDate' },
        { name: 'FileInfo' },
        { name: 'ExportExported', defaultValue: false }

    ]
});