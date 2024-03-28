Ext.define('B4.store.desktop.ReminderWidget', {
    extend: 'B4.base.Store',
    fields: ['Id', 'TypeReminder', 'CategoryReminder', 'CheckDate', 'Num', 'NumText', 'DocNum', 'ContragentName', 'Color', 'ColorTypeReminder'],
    autoLoad: false,
    storeId: 'storeReminderWidget',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder',
        listAction: 'ListWidgetInspector'
    }
});