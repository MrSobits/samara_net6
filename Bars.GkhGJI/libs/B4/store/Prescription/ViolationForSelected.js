Ext.define('B4.store.prescription.ViolationForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.prescription.Violation'],
    autoLoad: false,
    storeId: 'prescriptionViolationForSelectedStore',
    model: 'B4.model.prescription.Violation'
});