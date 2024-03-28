Ext.define('B4.store.deliveryagent.RealObjForAdd', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DeliveryAgent',
        listAction: 'ListRealObjForDelAgent'
    }
});