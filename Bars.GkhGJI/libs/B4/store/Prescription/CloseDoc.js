Ext.define('B4.store.prescription.CloseDoc', {
    extend: 'B4.base.Store',
    requires: ['B4.model.PrescriptionCloseDoc'],
    autoLoad: false,
    storeId: 'prescriptionCloseDocStore',
    model: 'B4.model.PrescriptionCloseDoc'
});