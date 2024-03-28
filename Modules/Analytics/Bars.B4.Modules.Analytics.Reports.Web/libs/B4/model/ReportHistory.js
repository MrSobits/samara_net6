Ext.define('B4.model.ReportHistory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ReportHistory'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ReportType' },
        { name: 'Date' },
        { name: 'Category', defaultValue: null },
        { name: 'Name' },
        { name: 'File', defaultValue: null },
        { name: 'User', defaultValue: null },
        { name: 'ReportId', defaultValue: null }       
    ]
});