Ext.define('B4.model.ActiveUser', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActiveUsers'
    },
    fields: [
        { name: 'Id', defaultValue: 0 },
        { name: 'TrackId' },
        { name: 'LastActivity' },
        { name: 'UserName' },
        { name: 'LastRequest' }
    ]
});