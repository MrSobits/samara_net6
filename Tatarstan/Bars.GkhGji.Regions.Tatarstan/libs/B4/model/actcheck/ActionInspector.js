Ext.define('B4.model.actcheck.ActionInspector', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckActionInspector'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheckAction' },
        { name: 'Inspector' }
    ]
});