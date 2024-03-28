Ext.define('B4.store.prescription.ViolationForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.prescription.Violation'],
    autoLoad: false,
    storeId: 'prescriptionViolationForSelectStore',
    model: 'B4.model.prescription.Violation'
});