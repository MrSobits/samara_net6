Ext.define('B4.model.WarningInspection', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'WarningInspection'
    },
    fields: [
        // grid
        { name: 'Id' },
        { name: 'State' },
        { name: 'Municipality' },
        { name: 'ContragentName' },
        { name: 'InspectionDate' },
        { name: 'RealityObjectCount' },
        { name: 'DocumentNumber' },
        { name: 'InspectionNumber' },
        { name: 'Inspectors' },
        { name: 'AppealNumber' },
        // edit
        { name: 'RegistrationNumberDate' },
        { name: 'CheckDayCount' },
        { name: 'CheckDate' },
        { name: 'InspectionBasis' },
        { name: 'Date' },
        { name: 'DocumentDate' },
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'SourceFormType' },
        { name: 'File' },
        { name: 'TypeBase', defaultValue: 20 },
        { name: 'RealityObjectCount' },
        { name: 'FormCheck', defaultValue: 10 },
        { name: 'IsDisposal' },
        { name: 'RealObjAddresses' },
        { name: 'State', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'ControlType', defaultValue: 10 },
        { name: 'AppealCits' },
        { name: 'InspectionBasis' },
        { name: 'AppealCitsNumberDate' },
        { name: 'WarningInspectionControlType' },
        { name: 'Inn'}
    ]
});