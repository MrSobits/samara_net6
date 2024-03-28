Ext.define('B4.model.RoleTypeHousePermission', {
    extend: 'B4.base.Model',
    idProperty: 'Code',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RoleTypeHousePermission'
    },
    fields: [
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Allowed' }
    ]
});