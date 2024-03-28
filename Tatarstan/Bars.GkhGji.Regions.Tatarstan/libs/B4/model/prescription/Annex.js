Ext.define('B4.model.prescription.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Prescription', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'SendFileToErknm', defaultValue: 10 }
    ]
});