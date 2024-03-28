Ext.define('B4.model.person.QualificationDocument', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QualificationDocument'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentType', defaultValue: null },
        { name: 'QualificationCertificate', defaultValue: null },
        { name: 'Number', defaultValue: null },
        { name: 'StatementNumber', defaultValue: null },
        { name: 'IssuedDate', defaultValue: null },
        { name: 'Document', defaultValue: null },
        { name: 'Note', defaultValue: null }
    ]
});