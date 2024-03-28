Ext.define('B4.store.Prescription', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.Prescription'],
    storeId: 'prescriptionStore',
    model: 'B4.model.Prescription'
});