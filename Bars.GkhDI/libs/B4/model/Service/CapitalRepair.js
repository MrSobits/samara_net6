Ext.define('B4.model.service.CapitalRepair', {
    extend: 'B4.model.service.Base',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CapRepairService'
    },
    fields: [
        { name: 'TypeOfProvisionService', defaultValue: 10 },
        { name: 'RegionalFund', defaultValue: 10 }
    ]
});