Ext.define('B4.model.mkdlicrequest.MKDLicRequestFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequestFile'
    },
    fields: [
        { name: 'Id' },
        { name: 'MKDLicRequest'},
        { name: 'FileInfo'},
        { name: 'SignedFile'},
        { name: 'DocumentName' },
        { name: 'LicStatementDocType', defaultValue: 0},
        { name: 'Description' },
        { name: 'DocDate' }
       
    ]
});