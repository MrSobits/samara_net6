Ext.define('B4.store.prescription.BaseDocument', {
    extend: 'B4.base.Store',
    requires: ['B4.model.prescription.BaseDocument'],
    autoLoad: false,
    storeId: 'prescriptionBaseDocumentStore',
    model: 'B4.model.prescription.BaseDocument'
});