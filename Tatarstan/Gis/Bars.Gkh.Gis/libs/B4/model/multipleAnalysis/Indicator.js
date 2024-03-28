Ext.define('B4.model.multipleAnalysis.Indicator', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    fields: [
        'Id',
        'Name',
        'MinValue',
        'MaxValue',
        'DeviationPercent',
        'ExactValue',
        'leaf'
    ],
    proxy: {
        url: 'MultipleAnalysisTemplate/GetIndicators',
        type: 'ajax'
    }
});