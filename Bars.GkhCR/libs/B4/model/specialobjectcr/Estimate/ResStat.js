Ext.define('B4.model.specialobjectcr.estimate.ResStat', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialResourceStatement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EstimateCalculation' },
        { name: 'Number' },
        { name: 'UnitMeasure' },
        { name: 'UnitMeasureName' },
        { name: 'Name' },
        { name: 'Reason' },
        { name: 'TotalCount', defaultValue: null },
        { name: 'TotalCost', defaultValue: null },
        { name: 'OnUnitCost', defaultValue: null }
    ]
});