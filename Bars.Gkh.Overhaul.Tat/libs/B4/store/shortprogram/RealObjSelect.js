Ext.define('B4.store.shortprogram.RealObjSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.shortprogram.RealityObject'],
    autoLoad: false,
    model: 'B4.model.shortprogram.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShortProgramRecord',
        listAction: 'ListForMassStateChange'
    }
});