Ext.define('B4.store.realityobj.Work', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Work'],
    autoLoad: false,
    storeId: 'realityObjectWorkStore',
    model: 'B4.model.dict.Work',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Work',
        listAction: 'ListWorksRealityObjectByPeriod'
    }
});