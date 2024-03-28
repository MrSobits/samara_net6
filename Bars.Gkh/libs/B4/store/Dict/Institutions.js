Ext.define('B4.store.dict.Institutions', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Institutions'],
    autoLoad: false,
    storeId: 'institutionsStore',
    model: 'B4.model.dict.Institutions'
});