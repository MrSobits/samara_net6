Ext.define('B4.model.PreventiveVisit', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveVisit'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'TypePreventiveAct', defaultValue: 10 },
        { name: 'Contragent', defaultValue: null },
        { name: 'DocumentName' },
        { name: 'PersonInspection', defaultValue: 10 },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'RealityObjectCount' },
        { name: 'PhysicalPersonAddress' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'PhysicalPersonINN' },
        { name: 'AccessGuid' },
        { name: 'AppealNumber' },
        { name: 'ActAddress' },
        { name: 'DispHeadHumber' },
        { name: 'Contragent' },
        { name: 'UsedDistanceTech', defaultValue: false },
        { name: 'DistanceDescription' },
        { name: 'DistanceCheckDate' },
        { name: 'TypeBase' },
        { name: 'VideoLink' },
        { name: 'KindKND' },
        { name: 'TypeBase2' },
        { name: 'TypeDocumentGji', defaultValue: 220 },
        { name: 'MunicipalityName' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'InspectionType', defaultValue: null },
        { name: 'SentToERKNM', defaultValue: false },
        { name: 'ERKNMID' },
        { name: 'ERKNMGUID' }
    ]
});