Ext.define('B4.model.smev.ERKNMFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ERKNMFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'ERKNM' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});