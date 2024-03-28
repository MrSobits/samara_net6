Ext.define('B4.store.ConstructionObjectForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ConstructionObject'],
    autoLoad: false,
    storeId: 'constructionObjectForSelectStore',
    model: 'B4.model.ConstructionObject'
});