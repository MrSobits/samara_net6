Ext.define('B4.model.DocumentGjiChildren', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentGjiChildren'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ChildrenId' },
        { name: 'ParentId' },
        { name: 'TypeDocumentGji', defaultValue: 10 },
        { name: 'DocumentId' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'DocumentNumber' },
        { name: 'DocumentSubNum' },
        { name: 'LiteralNum' },
        { name: 'DocumentYear' }
    ]
});