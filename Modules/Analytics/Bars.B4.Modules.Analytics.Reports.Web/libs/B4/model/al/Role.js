Ext.define('B4.model.al.Role', {
    extend: 'Ext.data.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StoredReport',
        listAction: 'GetRoles'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});