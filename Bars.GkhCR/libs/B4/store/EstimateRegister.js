Ext.define('B4.store.EstimateRegister', {
    extend: 'B4.base.Store',
    requires: ['B4.model.EstimateRegister'],
    autoLoad: false,
    storeId: 'estimateRegisterStore',
    model: 'B4.model.EstimateRegister',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EstimateCalculation'
    }
});