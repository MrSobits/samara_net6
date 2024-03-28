Ext.define('B4.store.BelayOrganization', {
    extend: 'B4.base.Store',
    requires: ['B4.model.BelayOrganization'],
    autoLoad: false,
    storeId: 'belayorgStore',
    model: 'B4.model.BelayOrganization'
});