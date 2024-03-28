Ext.define('B4.store.prescription.PrescriptionViolationForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.prescription.Violation'],
    model: 'B4.model.prescription.Violation',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionViol',
        listAction: 'ListPrescriptionViolation'
    }
});