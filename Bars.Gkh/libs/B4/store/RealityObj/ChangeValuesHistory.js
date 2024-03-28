Ext.define('B4.store.realityobj.ChangeValuesHistory', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'GetHistory'
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