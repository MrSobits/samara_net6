Ext.define('B4.model.administration.notify.Permission', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NotifyPermission'
    },
    fields: [
        { name: 'Id' },
        { name: 'Role' },
        { name: 'RoleName' },
        { name: 'Message' },
    ]
});