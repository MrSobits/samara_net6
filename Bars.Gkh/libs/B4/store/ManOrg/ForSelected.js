Ext.define('B4.store.manorg.ForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ManagingOrganization'],
    autoLoad: false,
    storeId: 'manorgForSelectedStore',
    model: 'B4.model.ManagingOrganization'
});