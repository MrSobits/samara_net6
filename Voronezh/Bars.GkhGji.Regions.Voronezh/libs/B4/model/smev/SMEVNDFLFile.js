Ext.define('B4.model.smev.SMEVNDFLFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVNDFLFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVNDFL' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});