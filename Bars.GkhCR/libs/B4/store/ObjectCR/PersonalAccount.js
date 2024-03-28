Ext.define('B4.store.objectcr.PersonalAccount', {
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.PersonalAccount'],
    autoLoad: false,
    storeId: 'personalAccountStore',
    model: 'B4.model.objectcr.PersonalAccount'
});