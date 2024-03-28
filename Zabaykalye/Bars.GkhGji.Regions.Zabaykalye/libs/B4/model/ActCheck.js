Ext.define('B4.model.ActCheck', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheck'
    },
    fields: [
        { name: 'TypeActCheck', defaultValue: 10 },
        { name: 'Inspection', defaultValue: null },
        { name: 'ResolutionProsecutor', defaultValue: null },
        { name: 'ActCheckGjiRealityObject', defaultValue: null },
        { name: 'ToProsecutor', defaultValue: 30 },
        { name: 'DateToProsecutor' },
        { name: 'Area' },
        { name: 'RealityObjectsList' },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 20 },
        { name: 'State', defaultValue: null },
        { name: 'MunicipalityNames' },
        { name: 'ContragentName' },
        { name: 'InspectorNames' },
        { name: 'RealityObjectCount' },
        { name: 'HasViolation' },
        { name: 'CountExecDoc' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'Flat' },
        { name: 'ActToPres', defaultValue: false}
    ]
});