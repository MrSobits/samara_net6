Ext.define('B4.store.realityobj.LiftRegister', {
    extend: 'B4.base.Store',
    requires: ['B4.model.realityobj.Lift'],
    autoLoad: false,
    storeId: 'roLiftRegisterStore',
    model: 'B4.model.realityobj.Lift',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListLiftsRegistry'
    }
});