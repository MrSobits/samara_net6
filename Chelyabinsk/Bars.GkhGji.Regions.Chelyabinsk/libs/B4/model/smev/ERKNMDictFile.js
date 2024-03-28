Ext.define('B4.model.smev.ERKNMDictFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ERKNMDictFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'ERKNMDict' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});