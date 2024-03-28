Ext.define('B4.model.actcheck.InspectedPart', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckInspectedPart'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheck', defaultValue: null },
        { name: 'InspectedPart', defaultValue: null },
        { name: 'Character' },
        { name: 'Description' }
    ]
});