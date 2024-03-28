Ext.define('B4.store.actcheck.RealObjForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObjectGji'],
    autoLoad: false,
    model: 'B4.model.RealityObjectGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheck',
        listAction: 'ListRealObjForActCheck'
    }
});