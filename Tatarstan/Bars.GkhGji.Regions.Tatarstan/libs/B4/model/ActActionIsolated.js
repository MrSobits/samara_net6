Ext.define('B4.model.ActActionIsolated', {
    extend: 'B4.model.ActCheck',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActActionIsolated'
    },
    fields: [
        { name: 'Inspectors' },
        { name: 'InspectorIds' }
    ]
});