Ext.define('B4.store.prescription.PrescriptionViolationForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.prescription.Violation'],
    model: 'B4.model.prescription.Violation',
    autoLoad: false,
});