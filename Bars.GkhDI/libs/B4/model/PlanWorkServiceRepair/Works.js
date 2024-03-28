Ext.define('B4.model.planworkservicerepair.Works', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PlanWorkServiceRepairWorks'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PlanWorkServiceRepair', defaultValue: null },
        { name: 'WorkRepairList', defaultValue: null },
        { name: 'WorkRepairListName', defaultValue: null },
        { name: 'DataComplete' },
        { name: 'PeriodicityTemplateService', defaultValue: null },
        { name: 'DateComplete', defaultValue: null },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'Cost', defaultValue: null },
        { name: 'FactCost', defaultValue: null },
        { name: 'ReasonRejection' },
        { name: 'PeriodicityTemplateServiceName' }
    ]
});