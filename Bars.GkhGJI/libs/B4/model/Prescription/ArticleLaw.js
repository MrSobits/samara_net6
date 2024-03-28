Ext.define('B4.model.prescription.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Prescription', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null },
        { name: 'Description' }
    ]
});