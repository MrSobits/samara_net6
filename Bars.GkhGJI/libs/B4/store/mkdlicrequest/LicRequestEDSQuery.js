Ext.define('B4.store.mkdlicrequest.LicRequestEDSQuery', {
    extend: 'B4.base.Store',
    requires: ['B4.model.mkdlicrequest.MKDLicRequestQuery'],
    autoLoad: false,
    storeId: 'licRequestEDSQueryStore',
    model: 'B4.model.mkdlicrequest.MKDLicRequestQuery'
});