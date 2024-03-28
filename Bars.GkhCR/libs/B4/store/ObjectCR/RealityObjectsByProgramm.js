Ext.define('B4.store.objectcr.RealityObjectsByProgramm', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'realityobjStore',
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectCr',
        listAction: 'RealityObjectByProgramm'
    }
});