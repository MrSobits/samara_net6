Ext.define('B4.model.mkdlicrequest.MKDLicRequestQuery', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequestQuery'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MKDLicRequest', defaultValue: null },
        { name: 'Inspector' },
        { name: 'CompetentOrg' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PerfomanceDate' },
        { name: 'PerfomanceFactDate' },
        { name: 'Description' },
        { name: 'File', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'Certificate', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'StatementDate' },
        { name: 'StatementNumber'},
        { name: 'SendDate' }
    ]
});