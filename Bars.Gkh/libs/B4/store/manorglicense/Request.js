Ext.define('B4.store.manorglicense.Request', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorglicense.Request'],
    autoLoad: false,
    storeId: 'manOrgLicenseRequestStore',
    model: 'B4.model.manorglicense.Request'
});