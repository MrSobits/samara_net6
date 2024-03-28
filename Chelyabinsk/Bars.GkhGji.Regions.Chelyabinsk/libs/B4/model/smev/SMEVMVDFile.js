Ext.define('B4.model.smev.SMEVMVDFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVMVDFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVMVD' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});