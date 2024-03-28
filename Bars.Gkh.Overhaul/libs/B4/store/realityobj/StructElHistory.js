Ext.define('B4.store.realityobj.StructElHistory', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElement',
        listAction: 'GetHistory',
        timeout: 5 * 60 * 1000
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