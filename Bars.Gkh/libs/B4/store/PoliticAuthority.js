Ext.define('B4.store.PoliticAuthority', {
    extend: 'B4.base.Store',
    requires: ['B4.model.PoliticAuthority'],
    autoLoad: false,
    storeId: 'politicAuthStore',
    model: 'B4.model.PoliticAuthority'
});