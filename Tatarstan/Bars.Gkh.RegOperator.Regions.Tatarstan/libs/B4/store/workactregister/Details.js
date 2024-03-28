Ext.define('B4.store.workactregister.Details', {
    extend: 'B4.base.Store',
    requires: ['B4.model.workactregister.Details'],
    autoLoad: false,
    storeId: 'workActRegisterDetailsStore',
    model: 'B4.model.workactregister.Details',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedWorkAct',
        listAction: 'ListDetails',
        timeout: 5 * 60 * 1000
    }
});