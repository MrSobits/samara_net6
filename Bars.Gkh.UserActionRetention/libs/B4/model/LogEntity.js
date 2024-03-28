Ext.define('B4.model.LogEntity', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LogEntity',
        timeout: 5 * 60 * 1000 // 5 минут
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EntityDateChange' },
        { name: 'EntityName' },
        { name: 'EntityId' },
        { name: 'EntityTypeChange' },
        { name: 'TypeAction' },
        { name: 'EntityDescription' },
        { name: 'UserId' },
        { name: 'UserName' },
        { name: 'UserLogin' },
        { name: 'Ip' }
    ]
});
