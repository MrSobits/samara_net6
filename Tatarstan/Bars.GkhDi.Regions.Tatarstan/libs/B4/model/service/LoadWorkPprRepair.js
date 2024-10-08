Ext.define('B4.model.service.LoadWorkPprRepair', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkRepairList',
        listAction: 'listselected'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ServiceId' },
        { name: 'GroupWorkPprId' },
        { name: 'Name' },
        { name: 'ServiceName' },
        { name: 'ProviderName' },
        { name: 'PlannedCost' },
        { name: 'FactCost' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'TypeColor' },
        { name: 'InfoAboutExec' },
        { name: 'ReasonRejection' }
    ]
});