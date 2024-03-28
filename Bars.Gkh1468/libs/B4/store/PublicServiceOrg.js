Ext.define('B4.store.PublicServiceOrg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.PublicServiceOrg'],
    autoLoad: false,
    storeId: 'publicservorgStore',
    model: 'B4.model.PublicServiceOrg'
});