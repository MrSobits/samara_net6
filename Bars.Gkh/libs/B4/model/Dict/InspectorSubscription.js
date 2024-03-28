Ext.define('B4.model.dict.InspectorSubscription', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectorSubscription'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Inspector' },
        { name: 'SignedInspector' },
        { name: 'Position' }
    ]
});