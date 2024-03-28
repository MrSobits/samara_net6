Ext.define('B4.model.VisitSheet', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VisitSheet'
    },
    fields: [
        { name: 'MunicipalityName' },
        { name: 'Address' },
        { name: 'ExecutingInspector' },
        { name: 'RealityObjectCount' },
        { name: 'VisitDateStart' },
        { name: 'HasViolation' },
        { name: 'TypeBase' },
        { name: 'InspectionId' }
    ]
});