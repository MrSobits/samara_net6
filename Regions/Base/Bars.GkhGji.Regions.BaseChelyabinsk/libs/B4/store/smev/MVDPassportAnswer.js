Ext.define('B4.store.smev.MVDPassportAnswer', {
    extend: 'B4.base.Store',
    fields: ['Id', 'TypeReminder', 'CategoryReminder', 'CheckDate', 'Num', 'NumText', 'DocNum', 'ContragentName', 'Color', 'ColorTypeReminder', 'AppealCorr', 'AppealCorrAddress', 'AppealDescription'],
    autoLoad: false,
    storeId: 'storeReminderWidget',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder',
        listAction: 'ListWidgetInspector'
    }
});