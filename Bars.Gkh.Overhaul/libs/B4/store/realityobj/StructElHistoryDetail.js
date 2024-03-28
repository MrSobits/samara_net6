Ext.define('B4.store.realityobj.StructElHistoryDetail', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElement',
        listAction: 'GetHistoryDetail'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PropertyName' },
        { name: 'OldValue' },
        { name: 'NewValue' },
        { name: 'Type' }
    ]
});