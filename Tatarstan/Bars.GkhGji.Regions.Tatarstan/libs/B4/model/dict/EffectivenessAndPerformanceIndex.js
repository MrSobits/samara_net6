Ext.define('B4.model.dict.EffectivenessAndPerformanceIndex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EffectivenessAndPerformanceIndex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'ParameterName' },
        { name: 'UnitMeasure' }
    ]
});