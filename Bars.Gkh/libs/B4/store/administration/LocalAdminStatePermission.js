Ext.define('B4.store.administration.LocalAdminStatePermission', {
    extend: 'B4.store.StatePermission',
    proxy: {
        type: 'ajax',
        url: B4.Url.action('/StatePermission/GetNodePermissions'),
        reader: {
            type: 'json'
        }
    }
});