Ext.define('B4.model.regressionanalysis.IndicatorType', {
    extend: 'Ext.data.Model',
    idProperty: 'id',    
    fields: [
        'id',
        'text',
        'EntityId'
    ],
    proxy: {
        url: 'RegressionAnalysis/GetIndicatorsTypes',
        type: 'ajax'
    }
});