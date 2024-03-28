Ext.define('B4.store.StatePermission', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.Url'],

    sortRoot: 'id',
    autoload: false,
    defaultRootId: 'root',
    proxy: {
        type: 'ajax',
        url: B4.Url.action('/StatePermission/GetRoleStatePermissions'),
        reader: {
            type: 'json'
        }
    },
    root: {
        text: 'root',
        expanded: false
    }
});