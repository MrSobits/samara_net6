Ext.define('B4.model.mkdlicrequest.MKDLicRequestQueryAnswer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequestQueryAnswer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MKDLicRequestQuery', defaultValue: null },    
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'File', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'Certificate', defaultValue: null },
        { name: 'SignedFile', defaultValue: null }
    ]
});