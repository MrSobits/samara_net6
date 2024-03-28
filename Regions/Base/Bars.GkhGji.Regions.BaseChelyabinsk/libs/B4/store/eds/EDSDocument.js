Ext.define('B4.store.eds.EDSDocument', {
    extend: 'B4.base.Store',
    requires: ['B4.model.eds.EDSDocument'],
    autoLoad: false,
    storeId: 'eDSDocumentStore',
    model: 'B4.model.eds.EDSDocument'
});