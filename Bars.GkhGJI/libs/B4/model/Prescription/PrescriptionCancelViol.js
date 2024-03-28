Ext.define('B4.model.prescription.PrescriptionCancelViol', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionCancelViolReference'
    },

    fields: [
        { name: 'Id', useNull: true },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'DatePlanRemoval' },
        { name: 'NewDatePlanRemoval' }
    ]
});