Ext.define('B4.model.import.OpenTatarstan', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ImportDataOt',
        listAction: 'OpenTatastanImportData'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectCreateDate' },
        { name: 'UserName' },
        { name: 'FileId' },
        { name: 'FileName' },
        { name: 'ImportResult' },
        { name: 'ImportName' },
        { name: 'ResponseCode' },
        { name: 'ResponseInfo' }
    ]
});