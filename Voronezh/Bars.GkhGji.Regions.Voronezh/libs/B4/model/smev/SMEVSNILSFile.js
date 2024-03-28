Ext.define('B4.model.smev.SMEVSNILSFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVSNILSFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVSNILS' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});