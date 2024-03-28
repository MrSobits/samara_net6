Ext.define('B4.model.service.WorkRepairDetail', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkRepairDetailTat'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'WorkPpr', defaultValue: null },
        { name: 'Name' },
        { name: 'UnitMeasure' },
        { name: 'FactVolume' },
        { name: 'PlannedVolume' }
    ]
});