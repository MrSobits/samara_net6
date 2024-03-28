Ext.define('B4.store.reminder.InspectorGji', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Reminder'],
    autoLoad: false,
    storeId: 'reminderInspectorGji',
    model: 'B4.model.Reminder',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder',
        listAction: 'ListReminderOfInspector',
        timeout: 999999
    }
});