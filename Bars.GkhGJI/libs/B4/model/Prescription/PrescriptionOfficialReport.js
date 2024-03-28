Ext.define('B4.model.prescription.PrescriptionOfficialReport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionOfficialReport'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Prescription', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'YesNo', defaultValue: 10 },  
        { name: 'OfficialReportType', defaultValue: 10 },  
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'State' },
        { name: 'ViolationDate' },
        { name: 'ExtensionViolationDate' },
        { name: 'DocumentNumber' },
        { name: 'Inspector' },
        { name: 'Description' }
    ]
});