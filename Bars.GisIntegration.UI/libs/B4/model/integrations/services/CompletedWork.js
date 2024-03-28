Ext.define('B4.model.integrations.services.CompletedWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Services',
        listAction: 'GetCompletedWorkList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' },
        { name: 'ActDate' },
        { name: 'ActNumber' }
    ]
});