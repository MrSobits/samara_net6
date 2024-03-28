Ext.define('B4.model.EffectivenessAndPerformanceIndexValue', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EffectivenessAndPerformanceIndexValue'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EffectivenessAndPerformanceIndex' },
        { name: 'EffectivenessAndPerformanceIndexName' },
        { name: 'CalculationStartDate' },
        { name: 'CalculationEndDate' },
        { name: 'Value' }
    ]
});