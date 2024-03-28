Ext.define('B4.store.transferrf.PersonalAccount', {
    extend: 'B4.base.Store',
    requires: ['B4.model.transferrf.PersonalAccount'],
    autoLoad: false,
    storeId: 'personalAccountStore',
    model: 'B4.model.transferrf.PersonalAccount'
});