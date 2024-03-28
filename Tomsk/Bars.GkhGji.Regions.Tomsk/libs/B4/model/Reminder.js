Ext.define('B4.model.Reminder', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InspectionGji', defaultValue: null },
        { name: 'DocumentGji', defaultValue: null },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'Inspector', defaultValue: null },
        { name: 'Actuality' },
        { name: 'TypeReminder', defaultValue: 10 },
        { name: 'CategoryReminder', defaultValue: 10 },
        { name: 'Num' },
        { name: 'CheckDate' },
        { name: 'CorrespondentAddress', defaultValue: null },
        { name: 'LocationProblem', defaultValue: null }
    ]
});