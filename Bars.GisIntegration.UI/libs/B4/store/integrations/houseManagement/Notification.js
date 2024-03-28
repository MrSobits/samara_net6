Ext.define('B4.store.integrations.houseManagement.Notification', {
    extend: 'B4.base.Store',
    requires: ['B4.model.integrations.houseManagement.Notification'],
    autoLoad: false,
    storeId: 'notificationStore',
    model: 'B4.model.integrations.houseManagement.Notification'
});
