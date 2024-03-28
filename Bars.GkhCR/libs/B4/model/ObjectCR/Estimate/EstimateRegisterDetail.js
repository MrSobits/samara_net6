Ext.define('B4.model.objectcr.estimate.EstimateRegisterDetail', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EstimateCalculation'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectCrId' },
        { name: 'State', defaultValue: null },
        { name: 'TypeWorkCrName' },
        { name: 'FinanceSourceName' },
        { name: 'TotalEstimateSum' },
        { name: 'TotalResourceSum' },
        { name: 'TotalEstimate' },
        { name: 'EstimationType' }
    ]
});