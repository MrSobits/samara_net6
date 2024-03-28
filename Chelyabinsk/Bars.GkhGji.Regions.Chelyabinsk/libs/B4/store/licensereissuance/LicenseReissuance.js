Ext.define('B4.store.licensereissuance.LicenseReissuance', {
    extend: 'B4.base.Store',
    requires: ['B4.model.licensereissuance.LicenseReissuance'],
    autoLoad: false,
    storeId: 'licenseReissuanceStore',
    model: 'B4.model.licensereissuance.LicenseReissuance'
});