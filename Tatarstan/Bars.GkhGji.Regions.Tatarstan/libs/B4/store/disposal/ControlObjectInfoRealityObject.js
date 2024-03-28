Ext.define('B4.store.disposal.ControlObjectInfoRealityObject', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObjectGji'],
    autoLoad: false,
    model: 'B4.model.RealityObjectGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionControlObjectInfo',
        listAction: 'ListInspGjiRealityObject'
    }
});