Ext.define('B4.model.administration.notify.Stats', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NotifyStats'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectCreateDate' },
        { name: 'ClickButton' },
        { name: 'Message' },
        { name: 'User' },
        { name: 'Login' },
        { name: 'Name' }
    ]
});