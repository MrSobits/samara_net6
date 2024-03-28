Ext.define('B4.model.preventiveaction.PreventiveActionTask',{
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveActionTask'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeDocumentGji', defaultValue: 220 },
        { name: 'Stage', defaultValue: 250 },
        { name: 'Municipality' },
        { name: 'InspectionId' },
        { name: 'TypeBase' },
        { name: 'ActionType'},
        { name: 'VisitType'},
        { name: 'CounselingType'},
        { name: 'ActionLocation'},
        { name: 'ActionStartDate'},
        { name: 'ActionEndDate'},
        { name: 'ActionStartTime'},
        { name: 'TaskingInspector'},
        { name: 'Executor'},
        { name: 'StructuralSubdivision'},
        { name: 'NotificationDate'},
        { name: 'OutgoingLetterDate'},
        { name: 'NotificationSent'},
        { name: 'NotificationType'},
        { name: 'NotificationDocumentNumber'},
        { name: 'OutgoingLetterNumber'},
        { name: 'NotificationReceived'},
        { name: 'ParticipationRejection'}
    ]
});