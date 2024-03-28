Ext.define('B4.model.cmnestateobj.StructuralElement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.PriceCalculateBy'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'StructuralElement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Group', defaultValue: null },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'LifeTime' },
        { name: 'LifeTimeAfterRepair' },
        { name: 'NormativeDoc', defaultValue: null },
        { name: 'MutuallyExclusiveGroup' },
        { name: 'CalculateBy', defaultValue: 0 },
        { name: 'ReformCode' }
    ]
});