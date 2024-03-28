Ext.define('B4.store.reminder.HeadGji', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Reminder'],
    autoLoad: false,
    storeId: 'reminderHeadGji',
    model: 'B4.model.Reminder',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder',
        listAction: 'ListReminderOfHead',
        timeout: 999999
    }
});