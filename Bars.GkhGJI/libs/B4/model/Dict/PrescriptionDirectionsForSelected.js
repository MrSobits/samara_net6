Ext.define('B4.model.dict.PrescriptionDirectionsForSelected', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Prescription'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Code' }
    ]
});