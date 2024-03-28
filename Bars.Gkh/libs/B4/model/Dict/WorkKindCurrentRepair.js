Ext.define('B4.model.dict.WorkKindCurrentRepair', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkKindCurrentRepair'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'TypeWork', defaultValue: 10 }
    ]
});