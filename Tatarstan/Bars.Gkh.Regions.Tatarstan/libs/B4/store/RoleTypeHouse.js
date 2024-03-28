Ext.define('B4.store.RoleTypeHouse', {
    extend: 'B4.base.Store',
    autoLoad: true,
    fields: ['Value', 'Name'],
    IdProperty: 'Value',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RoleTypeHousePermission',
        listAction: 'GetRoleTypeHouses'
    }
});