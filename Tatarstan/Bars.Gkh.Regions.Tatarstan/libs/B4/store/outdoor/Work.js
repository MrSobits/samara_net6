Ext.define('B4.store.outdoor.Work', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.WorkRealityObjectOutdoor'],
    autoLoad: false,
    storeId: 'outdoorWorkStore',
    model: 'B4.model.dict.WorkRealityObjectOutdoor',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkRealityObjectOutdoor',
        listAction: 'ListOutdoorWorksByPeriod'
    }
});