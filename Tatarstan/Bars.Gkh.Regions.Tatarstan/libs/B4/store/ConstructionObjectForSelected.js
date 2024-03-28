Ext.define('B4.store.ConstructionObjectForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ConstructionObject'],
    autoLoad: false,
    storeId: 'constructionObjectForSelectedStore',
    model: 'B4.model.ConstructionObject'
});