Ext.define('B4.model.dict.ConstructiveElement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'constructiveelement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Group' },
        { name: 'GroupName' },
        { name: 'Lifetime' },
        { name: 'NormativeDocName' },
        { name: 'NormativeDoc' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'RepairCost' }
    ]
});