Ext.define('B4.model.preventiveaction.visit.Violation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VisitSheetViolation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ViolationId' },
        { name: 'NormativeDocNames' },
        { name: 'Name' },
        { name: 'IsThreatToLegalProtectedValues' }
    ]
});