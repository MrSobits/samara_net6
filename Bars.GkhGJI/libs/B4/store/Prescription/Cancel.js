Ext.define('B4.store.prescription.Cancel', {
    extend: 'B4.base.Store',
    requires: ['B4.model.prescription.Cancel'],
    autoLoad: false,
    storeId: 'prescriptionCancelStore',
    model: 'B4.model.prescription.Cancel'
});