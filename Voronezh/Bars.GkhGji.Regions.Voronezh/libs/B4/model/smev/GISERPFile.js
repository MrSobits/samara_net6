Ext.define('B4.model.smev.GISERPFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GISERPFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'GISERP' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});