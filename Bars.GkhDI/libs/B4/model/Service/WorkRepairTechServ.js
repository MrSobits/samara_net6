Ext.define('B4.model.service.WorkRepairTechServ', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkRepairTechServ'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'WorkTo', defaultValue: null },
        { name: 'Name' },
        { name: 'GroupName' }
    ]
});