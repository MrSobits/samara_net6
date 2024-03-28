Ext.define('B4.store.objectcr.estimate.EstimateRegisterDetail', {
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.estimate.EstimateRegisterDetail'],
    autoLoad: false,
    storeId: 'estimateRegisterDetailStore',
    model: 'B4.model.objectcr.estimate.EstimateRegisterDetail',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EstimateCalculation',
        listAction: 'ListEstimateRegisterDetail'
    }
});