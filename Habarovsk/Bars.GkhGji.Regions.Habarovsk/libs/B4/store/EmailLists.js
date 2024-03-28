Ext.define('B4.store.EmailLists', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.EmailLists'],
    storeId: 'emailListsStore',
    model: 'B4.model.EmailLists'
});