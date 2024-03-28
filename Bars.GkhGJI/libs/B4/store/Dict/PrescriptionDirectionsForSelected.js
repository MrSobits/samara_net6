Ext.define('B4.store.dict.PrescriptionDirectionsForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.PrescriptionDirectionsForSelected'],
    autoLoad: false,
    storeId: 'prescriptionDirectionsForSelected',
    model: 'B4.model.dict.PrescriptionDirectionsForSelected',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Prescription',
        listAction: 'ListDirections'
    }
});
