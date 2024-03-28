Ext.define('B4.store.ManagingOrganization', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ManagingOrganization'],
    autoLoad: false,
    storeId: 'manorgStore',
    model: 'B4.model.ManagingOrganization'
});