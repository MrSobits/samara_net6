Ext.define('B4.store.prescription.ForSelected', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.Prescription'],
    storeId: 'prescriptionForSelectedStore',
    model: 'B4.model.Prescription'
});