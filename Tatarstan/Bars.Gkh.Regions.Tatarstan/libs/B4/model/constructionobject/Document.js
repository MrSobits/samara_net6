Ext.define('B4.model.constructionobject.Document', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'constructionobjectdocument',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ConstructionObject', defaultValue: null },
        { name: 'Type', defaultValue: 10 },
        { name: 'Name' },
        { name: 'Date' },
        { name: 'Number' },
        { name: 'File', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'Description' }
    ]
});