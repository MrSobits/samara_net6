Ext.define('B4.model.preventiveaction.visit.ViolationInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VisitSheetViolationInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'VisitSheet' },
        { name: 'RealityObject' },
        { name: 'RealityObjectId' },
        { name: 'Address' },
        { name: 'Violation' },
        { name: 'Description' }
    ]
});