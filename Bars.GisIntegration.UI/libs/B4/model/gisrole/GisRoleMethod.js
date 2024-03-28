Ext.define('B4.model.gisrole.GisRoleMethod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisRoleMethod'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Role', defaultValue: null },
        { name: 'MethodName' },
        { name: 'MethodId' }
    ]
});
