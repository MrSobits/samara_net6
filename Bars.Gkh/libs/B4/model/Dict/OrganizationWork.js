Ext.define('B4.model.dict.OrganizationWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.TypeWork', 'B4.enums.WorkAssignment'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'Work',
        timeout: 5 * 60 * 1000 // 5 минут
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'WorkAssignment' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'UnitMeasureName' },
        { name: 'Description' },
        { name: 'IsAdditionalWork', defaultValue: false },
        { name: 'TypeWork', defaultValue: 10 }
    ]
});
