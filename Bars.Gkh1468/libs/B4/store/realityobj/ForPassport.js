Ext.define('B4.store.realityobj.ForPassport', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject1468',
        listAction: 'ListForPassport'
    }
});