Ext.define('B4.model.dict.WorkPrice', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkPrice'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'Job', defaultValue: null },
        { name: 'NormativeCost' },
        { name: 'SquareMeterCost' },
        { name: 'Year' },
        { name: 'Municipality' },
        { name: 'JobName' },
        { name: 'CapitalGroup' },
        { name: 'ParentMo' }
    ]
});