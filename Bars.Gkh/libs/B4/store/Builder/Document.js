Ext.define('B4.store.builder.Document', {
    extend: 'B4.base.Store',
    requires: ['B4.model.builder.Document'],
    autoLoad: false,
    storeId: 'builderDocumentStore',
    model: 'B4.model.builder.Document'
});