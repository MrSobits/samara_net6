Ext.define('B4.model.preventiveaction.PreventiveAction',{
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveAction'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeDocumentGji', defaultValue: 190 },
        { name: 'Stage', defaultValue: 220 },
        { name: 'Municipality' },
        { name: 'ActionType' },
        { name: 'VisitType' },
        { name: 'ControlledOrganization' },
        { name: 'InspectionId' },
        { name: 'ControlledPersonType' },
        { name: 'FullName' },
        { name: 'PhoneNumber' },
        { name: 'FileName' },
        { name: 'FileNumber' },
        { name: 'FileDate' },
        { name: 'File' },
        { name: 'ControlledPersonAddress' },
        { name: 'Head' },
        { name: 'GjiContragentName'},
        { name: 'MunicipalityName'},
        { name: 'Inspectors'},
        { name: 'InspectorIds'},
        { name: 'Plan'},
        { name: 'ZonalInspection'},
        { name: 'ControlType'},
        { name: 'ErknmGuid'},
        { name: 'ErknmRegistrationNumber'},
        { name: 'ErknmRegistrationDate'},
        { name: 'SentToErknm'}
    ]
});