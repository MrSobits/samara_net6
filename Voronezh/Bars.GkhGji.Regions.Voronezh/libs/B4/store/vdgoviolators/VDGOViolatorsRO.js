Ext.define('B4.store.vdgoviolators.VDGOViolatorsRO', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'VDGOViolatorsROStore',
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VDGOViolators',
        listAction: 'GetListRO'
    }
});