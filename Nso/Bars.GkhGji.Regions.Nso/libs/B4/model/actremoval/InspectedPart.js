Ext.define('B4.model.actremoval.InspectedPart', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActRemovalInspectedPart'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActRemoval', defaultValue: null },
        { name: 'InspectedPart', defaultValue: null },
        { name: 'Character' },
        { name: 'Description' }
    ]
});