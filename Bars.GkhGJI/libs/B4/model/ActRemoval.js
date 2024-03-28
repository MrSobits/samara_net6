Ext.define('B4.model.ActRemoval', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActRemoval'
    },
    fields: [
        { name: 'TypeRemoval', defaultValue: 20 },
        { name: 'Inspection', defaultValue: null },
        { name: 'Description' },
        { name: 'ParentDocumentName' },
        { name: 'TypeDocumentGji', defaultValue: 30 },
        { name: 'CountExecDoc' },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'InspectorNames' },
        { name: 'State', defaultValue: null },
        { name: 'MunicipalityNames' },
        { name: 'ParentContragentName' },
        { name: 'RealityObjectCount' },
        { name: 'ControlType' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'Area', defaultValue: null },
        { name: 'Flat' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});