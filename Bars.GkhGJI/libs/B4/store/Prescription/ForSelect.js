Ext.define('B4.store.prescription.ForSelect', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.Prescription'],
    storeId: 'prescriptionForSelectStore',
    model: 'B4.model.Prescription'
});