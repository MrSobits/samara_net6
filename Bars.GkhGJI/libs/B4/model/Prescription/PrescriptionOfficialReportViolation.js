Ext.define('B4.model.prescription.PrescriptionOfficialReportViolation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionOfficialReportViolation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PrescriptionOfficialReport', defaultValue: null },
        { name: 'ViolationGjiPin' },
        { name: 'ViolationGji' },
        { name: 'Action' },
        { name: 'Description' },
        { name: 'DatePlanRemoval' },
        { name: 'DatePlanExtension' },
        { name: 'NotificationDate' },
        { name: 'DateFactRemoval' },
        { name: 'PrescriptionViol', defaultValue: null }
    ]
});