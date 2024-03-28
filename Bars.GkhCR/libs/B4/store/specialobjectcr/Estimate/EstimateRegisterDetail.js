Ext.define('B4.store.specialobjectcr.estimate.EstimateRegisterDetail', {
    extend: 'B4.base.Store',
    requires: ['B4.model.specialobjectcr.estimate.EstimateRegisterDetail'],
    autoLoad: false,
    model: 'B4.model.specialobjectcr.estimate.EstimateRegisterDetail',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialEstimateCalculation',
        listAction: 'ListEstimateRegisterDetail'
    }
});