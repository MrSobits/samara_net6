Ext.define('B4.model.importexport.IncrementalImport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ImportData',
        listAction: 'LoadedFileList'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectCreateDate' },
        { name: 'ObjectVersion' },
        { name: 'UserId' },
        { name: 'FileName' },
        { name: 'TypeStatus' },
        { name: 'FileId' },
        { name: 'LogId' }
    ]
});
