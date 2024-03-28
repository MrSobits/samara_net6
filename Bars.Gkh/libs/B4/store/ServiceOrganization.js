Ext.define('B4.store.ServiceOrganization', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ServiceOrganization'],
    autoLoad: false,
    storeId: 'servorgStore',
    model: 'B4.model.ServiceOrganization'
});