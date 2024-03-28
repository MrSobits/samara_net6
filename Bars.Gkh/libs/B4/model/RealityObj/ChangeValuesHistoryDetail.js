Ext.define('B4.model.realityobj.ChangeValuesHistoryDetail', {
    extend: 'B4.base.Model',
    idProperty: 'id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
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