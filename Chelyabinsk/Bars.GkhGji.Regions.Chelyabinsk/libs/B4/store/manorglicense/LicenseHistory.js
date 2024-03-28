Ext.define('B4.store.manorglicense.LicenseHistory', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorglicense.LicenseHistory'],
    autoLoad: false,
    storeId: 'manOrgLicenseHistoryStore',
    model: 'B4.model.manorglicense.LicenseHistory'
});