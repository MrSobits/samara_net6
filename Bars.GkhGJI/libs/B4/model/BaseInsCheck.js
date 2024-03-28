Ext.define('B4.model.BaseInsCheck', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseInsCheck'
    },
    fields: [
        { name: 'Municipality', defaultValue: null },
        { name: 'Plan', defaultValue: null },
        { name: 'RealityObjectId', defaultValue: null },
        { name: 'InspectorId', defaultValue: null },
        { name: 'InspectionNumber' },
        { name: 'InsCheckDate' },
        { name: 'DateStart' },
        { name: 'Area' },
        { name: 'Reason' },
        { name: 'ContragentName' },
        { name: 'TypeBase', defaultValue: 10 },
        { name: 'TypeFact', defaultValue: 10 },
        { name: 'InspectionDate' },
        { name: 'DisposalNumber' },
        { name: 'InspectorNames' },
        { name: 'Address' },
        { name: 'TypeDocument', defaultValue: 10 },
        { name: 'DocumentDate' },
        { name: 'DocFile' },
        { name: 'DocumentNumber' },
        { name: 'State', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});