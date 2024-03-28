Ext.define('B4.model.DocumentGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentId' },
        { name: 'State', defaultValue: null },
        { name: 'Stage', defaultValue: null },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'DocumentNumber', defaultValue: null },
        { name: 'DocumentNum', defaultValue: null },
        { name: 'DocumentSubNum', defaultValue: null },
        { name: 'LiteralNum', defaultValue: null },
        { name: 'DocumentYear', defaultValue: null },
        { name: 'TypeDocumentGji', defaultValue: 10 }
    ]
});