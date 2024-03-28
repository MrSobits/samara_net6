Ext.define('B4.model.businessactivity.ServiceJuridicalGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceJuridicalGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BusinessActivityNotif', defaultValue: null },
        { name: 'KindWorkNotificationGji', defaultValue: null },
        { name: 'KindWorkNotifGjiName' }
    ]
});