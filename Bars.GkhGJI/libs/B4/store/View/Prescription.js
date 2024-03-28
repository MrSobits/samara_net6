Ext.define('B4.store.view.Prescription', {
    requires: ['B4.model.Prescription'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewPrescriptionStore',
    model: 'B4.model.Prescription',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Prescription',
        listAction: 'ListView'
    }
});