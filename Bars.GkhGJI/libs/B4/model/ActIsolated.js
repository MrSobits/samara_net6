Ext.define('B4.model.ActIsolated', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolated'
    },
    fields: [
        { name: 'Area' },
        { name: 'Flat' },
        { name: 'DocumentTime' },
        { name: 'State', defaultValue: null },
        { name: 'DocumentPlaceFias', defaultValue: null },
        { name: 'TypeDocumentGji', defaultValue: 20 },
        { name: 'MunicipalityNames' },
        { name: 'MunicipalityId' },
        { name: 'ContragentName' },
        { name: 'InspectorNames' },
        { name: 'RealityObjectCount' },
        { name: 'HasViolation' },
        { name: 'CountExecDoc' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});