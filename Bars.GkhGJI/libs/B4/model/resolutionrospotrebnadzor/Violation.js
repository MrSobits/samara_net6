Ext.define('B4.model.resolutionrospotrebnadzor.Violation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionRospotrebnadzorViolation'
    },
    fields: [
        { name: 'Id' },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'ViolationGjiName', defaultValue: null },
        { name: 'DatePlanRemoval' },
        { name: 'DateFactRemoval' },
        { name: 'Resolution', useNull: false },
        { name: 'Description' }
    ]
});