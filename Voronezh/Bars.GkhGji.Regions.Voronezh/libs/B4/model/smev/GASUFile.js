Ext.define('B4.model.smev.GASUFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GASUFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'GASU' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});