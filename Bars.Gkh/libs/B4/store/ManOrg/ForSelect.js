Ext.define('B4.store.manorg.ForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ManagingOrganization'],
    autoLoad: false,
    storeId: 'manorgForSelectStore',
    model: 'B4.model.ManagingOrganization'
});