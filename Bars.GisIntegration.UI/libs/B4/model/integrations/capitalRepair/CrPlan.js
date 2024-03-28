Ext.define('B4.model.integrations.capitalrepair.CrPlan', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CapitalRepair',
        listAction: 'GetCapitalRepairPlan'
    },
    fields: [
        { name: 'MunicipalityName' },
        { name: 'StartMonthYear', type: 'date' },
        { name: 'Name' }
    ]
});