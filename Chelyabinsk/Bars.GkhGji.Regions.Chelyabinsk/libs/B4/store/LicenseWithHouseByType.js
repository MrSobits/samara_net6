Ext.define('B4.store.LicenseWithHouseByType', {
    extend: 'B4.base.Store',
    requires: ['B4.model.LicenseWithHouseByType'],
    autoLoad: false,
    storeId: 'licenseWithHousebyTypeStore',
    model: 'B4.model.LicenseWithHouseByType'
});