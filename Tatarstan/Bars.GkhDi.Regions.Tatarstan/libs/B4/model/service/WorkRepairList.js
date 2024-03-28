Ext.define('B4.model.service.WorkRepairList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkRepairListTat'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'GroupWorkPpr', defaultValue: null },
        { name: 'GroupWorkPprId', defaultValue: null },
        { name: 'Name' },
        { name: 'PlannedCost', defaultValue: null },
        { name: 'FactCost', defaultValue: null },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'InfoAboutExec' },
        { name: 'ReasonRejection' }
    ]
});