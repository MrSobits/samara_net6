Ext.define('B4.model.integrations.houseManagement.Notification', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseManagement',
        listAction: 'GetNotificationList'
    },
    fields: [
        { name: 'NotifTopic' },
        { name: 'NotifContent' },
        { name: 'IsImportant' },
        { name: 'Address' },
        { name: 'NotifFrom' },
        { name: 'NotifTo' }
    ]
});
