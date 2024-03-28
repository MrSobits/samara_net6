Ext.define('B4.model.tasks.TaskEntry', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskEntry',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ParentId' },
        { name: 'UserLogin' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'Status' },
        { name: 'UserId' },
        { name: 'CreateDate' },
        { name: 'Duration' },
        { name: 'Progress' },
        { name: 'Percentage' }
    ]
});