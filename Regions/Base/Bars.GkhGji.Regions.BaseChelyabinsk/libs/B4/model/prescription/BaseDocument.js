Ext.define('B4.model.prescription.BaseDocument', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionBaseDocument'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Prescription' },
        { name: 'KindBaseDocument' },
        { name: 'DateDoc' },
        { name: 'NumDoc' }
    ]
});