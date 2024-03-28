Ext.define('B4.store.actcheck.RealityObjectByInspection', {
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'actcheckRealityObjectByInspectionStore',
    fields: ['Id', 'Municipality', 'Address'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheck',
        listAction: 'GetListRealityObjectByInspection'
    }
});