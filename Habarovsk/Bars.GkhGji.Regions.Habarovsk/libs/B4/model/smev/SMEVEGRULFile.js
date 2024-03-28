Ext.define('B4.model.smev.SMEVEGRULFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEGRULFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVEGRUL' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});