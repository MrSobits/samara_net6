Ext.define('B4.model.service.Repair', {
    extend: 'B4.model.service.Base',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RepairService'
    },
    fields: [
        { name: 'TypeOfProvisionService', defaultValue: 10 },
        { name: 'SumWorkTo', defaultValue: null }
    ]
});