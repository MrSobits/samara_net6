Ext.define('B4.model.FormPermission', {
    extend: 'B4.base.Model',
    idProperty: 'PermissionIds',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormPermission',
        listAction: 'GetFormPermissions'
    },
    fields: [
        { name: 'PermissionId' },
        { name: 'Path' },
        { name: 'Name' },
        { name: 'Grant' }
    ]
});