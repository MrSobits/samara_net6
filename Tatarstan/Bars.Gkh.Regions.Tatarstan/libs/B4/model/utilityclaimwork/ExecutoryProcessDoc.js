Ext.define('B4.model.utilityclaimwork.ExecutoryProcessDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ExecutoryProcessDocument'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ExecutoryProcess', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'ExecutoryProcessDocumentType', defaultValue: 10},
        { name: 'Date' },
        { name: 'Number' },
        { name: 'Notation' }
    ]
});