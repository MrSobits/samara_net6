Ext.define('B4.store.prescription.Relations', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.DocumentGjiChildren'],
    storeId: 'prescriptionRelationsStore',
    model: 'B4.model.DocumentGjiChildren'
});