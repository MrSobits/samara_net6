Ext.define('B4.model.Role', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'role'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'LocalAdmin', useNull: true },
        { name: 'RoleList', useNull: true },
    ]
});