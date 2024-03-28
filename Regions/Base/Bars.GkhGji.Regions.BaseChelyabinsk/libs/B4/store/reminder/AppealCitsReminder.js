Ext.define('B4.store.reminder.AppealCitsReminder', {
    extend: 'B4.base.Store',
    fields: [
        { name: 'Id' },
        { name: 'AppealCits' },
        { name: 'Inspector' },
        { name: 'Num' },
        { name: 'AppealState' },
        { name: 'NumberGji' },
        { name: 'CheckDate' },
        { name: 'SoprDate' },
        { name: 'CheckTime' },
        { name: 'ExtensTime' },
        { name: 'AppealCorr' },
        { name: 'Contragent' },
        { name: 'AppealCorrAddress' },
        { name: 'AppealDescription' },
        { name: 'CheckingInspector' },
        { name: 'Guarantor' },
        { name: 'State', defaultValue: null },
        { name: 'StatementSubjects' },
        { name: 'IncomingSources' },
        { name: 'AppealCitsExecutant' },
        { name: 'DateFrom' },
        { name: 'HasAppealCitizensInWorkState' }
    ],
    autoLoad: false,
    storeId: 'appealCitsReminderStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder',
        listAction: 'ListAppealCitsReminder'
    }
});