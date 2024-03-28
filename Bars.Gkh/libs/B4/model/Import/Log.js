Ext.define('B4.model.Import.Log', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ImportLog',
        timeout: 120000
    },  
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Operator', defaultValue: null },
        { name: 'StartDate' },
        { name: 'UploadDate' },
        { name: 'TaskStatus' },
        { name: 'FileName' },
        { name: 'ImportKey' },
        { name: 'CountWarning' },
        { name: 'CountError' },
        { name: 'CountImportedRows' },
        { name: 'CountChangedRows' },
        { name: 'CountImportedFile' },
        { name: 'File', defaultValue: null  },
        { name: 'LogFile', defaultValue: null }
    ]
});