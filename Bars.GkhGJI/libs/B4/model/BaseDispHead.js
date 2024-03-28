Ext.define('B4.model.BaseDispHead', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseDispHead'
    },
    fields: [
        { name: 'Municipality', defaultValue: null },
        { name: 'Head', defaultValue: null },
        { name: 'PrevDocument', defaultValue: null },
        { name: 'ContragentName', defaultValue: null },
        { name: 'InspectionNumber' },
        { name: 'DispHeadDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'MunicipalityNames' },
        { name: 'TypeBaseDispHead', defaultValue: 10 },
        { name: 'TypeForm', defaultValue: 10 },
        { name: 'TypeBase', defaultValue: 40 },
        { name: 'RealityObjectCount' },
        { name: 'DisposalNumber' },
        { name: 'InspectorNames' },
        { name: 'DispHeadNumber' },
        { name: 'DisposalTypeSurveys' },
        { name: 'File', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'Inn'}
    ]
});