Ext.define('B4.store.reminder.AppealCitsReminder', {
    extend: 'B4.base.Store',
    fields: ['Id', 'AppealCits', 'Inspector', 'Num', 'CheckDate', 'AppealCorr', 'ExtensTime', 'AppealCorrAddress', 'AppealDescription'],
    autoLoad: false,
    storeId: 'appealCitsReminderStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder',
        listAction: 'ListAppealCitsReminder'
    }
});