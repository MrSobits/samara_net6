Ext.define('B4.model.smev.GisGmpFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGmpFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'GisGmp' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});