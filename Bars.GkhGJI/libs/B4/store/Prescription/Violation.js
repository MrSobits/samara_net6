Ext.define('B4.store.prescription.Violation', {
    extend: 'B4.base.Store',
    requires: ['B4.model.prescription.Violation'],
    autoLoad:false,
    storeId: 'prescriptionViolationStore',
    model: 'B4.model.prescription.Violation'
});