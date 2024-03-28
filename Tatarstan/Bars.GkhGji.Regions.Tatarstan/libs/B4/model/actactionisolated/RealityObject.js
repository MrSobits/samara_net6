Ext.define('B4.model.actactionisolated.RealityObject', {
    extend: 'B4.model.RealityObject',
    requires: ['B4.model.RealityObject'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActActionIsolated',
        listAction: 'GetRealityObjectsList'
    }
});