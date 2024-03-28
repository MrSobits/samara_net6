Ext.define('B4.model.service.LoadWorkToRepair', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkRepairTechServ',
        listAction: 'listselected'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'RepairServiceId' },
        { name: 'ProviderName' },
        { name: 'SumWorkTo' },
        { name: 'TypeColor' }
    ]
});