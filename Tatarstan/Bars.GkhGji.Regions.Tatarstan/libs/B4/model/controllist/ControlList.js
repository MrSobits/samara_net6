Ext.define('B4.model.controllist.ControlList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlList'
    },
    fields: [
        { name: 'TorId' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'File', defaultValue: null },
        { name: 'Disposal' },
        { name: 'Name' },
        { name: 'ApprovalDetails' }
    ]
});