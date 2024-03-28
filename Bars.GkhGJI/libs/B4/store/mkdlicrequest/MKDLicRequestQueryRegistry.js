Ext.define('B4.store.mkdlicrequest.MKDLicRequestQueryRegistry', {
    extend: 'B4.base.Store',
    requires: ['B4.model.mkdlicrequest.MKDLicRequestQuery'],
    autoLoad: false,
    storeId: 'mkdLicRequestQueryRegistryStore',
    model: 'B4.model.mkdlicrequest.MKDLicRequestQuery'
});