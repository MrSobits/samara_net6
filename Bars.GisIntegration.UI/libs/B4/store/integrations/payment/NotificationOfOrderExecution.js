Ext.define('B4.store.integrations.payment.NotificationOfOrderExecution', {
    extend: 'B4.base.Store',
    requires: ['B4.model.integrations.payment.NotificationOfOrderExecution'],
    autoLoad: false,
    storeId: 'notificationOfOrderExecutionStore',
    model: 'B4.model.integrations.payment.NotificationOfOrderExecution'
});