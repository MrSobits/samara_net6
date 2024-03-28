Ext.define('B4.model.smev.SMEVDISKVLICFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVDISKVLICFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVDISKVLIC' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});