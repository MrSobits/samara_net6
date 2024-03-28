Ext.define('B4.store.mkdlicrequest.MKDLicRequestQueryAnswerRegistry', {
    extend: 'B4.base.Store',
    requires: ['B4.model.mkdlicrequest.MKDLicRequestQueryAnswer'],
    autoLoad: false,
    storeId: 'mkdLicRequestQueryAnswerRegistryStore',
    model: 'B4.model.mkdlicrequest.MKDLicRequestQueryAnswer'
});