Ext.define('B4.model.administration.LocalAdminRole', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LocalAdminRole'
    },
    fields: [
        { name: 'Id', useNull: false },
        { name: 'Name' },
        { name: 'ChildRoles', defaultValue: []},
    ]
});