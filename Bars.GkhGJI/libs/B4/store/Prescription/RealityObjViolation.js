Ext.define('B4.store.prescription.RealityObjViolation', {
    extend: 'B4.base.Store',
    requires: ['B4.model.prescription.Violation'],
    autoLoad: false,
    storeId: 'prescriptionRealityObjViolationStore',
    model: 'B4.model.prescription.Violation',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionViol',
        listAction: 'ListRealityObject'
    }
});