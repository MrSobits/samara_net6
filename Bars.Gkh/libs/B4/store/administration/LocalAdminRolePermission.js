Ext.define('B4.store.administration.LocalAdminRolePermission', {
    extend: 'B4.store.Permission',
    proxy: {
        type: 'ajax',
        url: B4.Url.action('/GkhPermission/GetNodePermissions'),
        reader: {
            type: 'json'
        }
    }
});