Ext.define('B4.model.smev.SMEVEDOFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEDOFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVEDO' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});