Ext.define('B4.model.ActVisual', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActVisual'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'InspectionResult' },
        { name: 'Conclusion' },
        { name: 'Flat' },
        { name: 'Hour' },
        { name: 'Minute' },
        { name: 'TypeBase' },
        { name: 'FrameVerification' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeDocumentGji' }
    ]
});