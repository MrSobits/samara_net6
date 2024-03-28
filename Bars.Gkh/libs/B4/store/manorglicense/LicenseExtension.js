Ext.define('B4.store.manorglicense.LicenseExtension', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorglicense.LicenseExtension'],
    autoLoad: false,
    storeId: 'manOrgLicenseExtensionStore',
    model: 'B4.model.manorglicense.LicenseExtension'
});