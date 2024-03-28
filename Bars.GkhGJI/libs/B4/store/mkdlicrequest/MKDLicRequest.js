Ext.define('B4.store.mkdlicrequest.MKDLicRequest', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.mkdlicrequest.MKDLicRequest'],
    storeId: 'mKDLicRequestStore',
    model: 'B4.model.mkdlicrequest.MKDLicRequest'
});