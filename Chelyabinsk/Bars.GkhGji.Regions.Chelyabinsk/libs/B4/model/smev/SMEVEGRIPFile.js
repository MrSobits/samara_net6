Ext.define('B4.model.smev.SMEVEGRIPFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEGRIPFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVEGRIP' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});