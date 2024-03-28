Ext.define('B4.store.manorglicense.License', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorglicense.License'],
    autoLoad: false,
    storeId: 'manOrgLicenseStore',
    model: 'B4.model.manorglicense.License'
});