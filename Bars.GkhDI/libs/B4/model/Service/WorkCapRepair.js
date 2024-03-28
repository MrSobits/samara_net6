Ext.define('B4.model.service.WorkCapRepair', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkCapRepair'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'Work', defaultValue: null },
        { name: 'PlannedVolume', defaultValue: null },
        { name: 'PlannedCost', defaultValue: null },
        { name: 'FactedVolume', defaultValue: null },
        { name: 'FactedCost', defaultValue: null },
        { name: 'Name' },
        { name: 'WorkId' }
    ]
});