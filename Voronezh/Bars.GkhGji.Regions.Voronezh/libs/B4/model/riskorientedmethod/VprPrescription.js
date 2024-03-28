Ext.define('B4.model.riskorientedmethod.VprPrescription', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VprPrescription'
    },
    fields: [
        { name: 'Id' },
        { name: 'ROMCategory' },
        { name: 'Prescription' },
        { name: 'PrescriptionNum' },
        { name: 'PrescriptionDate' },
        { name: 'StateName' },
    ]
});