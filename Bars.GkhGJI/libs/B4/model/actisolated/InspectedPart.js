Ext.define('B4.model.actisolated.InspectedPart', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedInspectedPart'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolated', defaultValue: null },
        { name: 'InspectedPart', defaultValue: null },
        { name: 'Character' },
        { name: 'Description' }
    ]
});