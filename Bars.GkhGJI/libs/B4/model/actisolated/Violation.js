Ext.define('B4.model.actisolated.Violation', {
    extend: 'B4.model.inspectiongji.ViolStage',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedRealObjViolation'
    },
    fields: [
        { name: 'ActIsolatedRealObj', defaultValue: null },
        { name: 'EventResult' },
        { name: 'ViolationGjiName' },
        { name: 'DateCancel' },
        { name: 'ViolationGjiId' }
    ]
});