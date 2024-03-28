Ext.define('B4.model.smev.SMEVEGRNFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEGRNFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVEGRN' },
        { name: 'SMEVFileType' },
        { name: 'Name' },
        { name: 'FileInfo' }
    ]
});