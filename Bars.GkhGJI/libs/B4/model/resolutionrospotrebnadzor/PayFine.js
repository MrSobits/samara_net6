Ext.define('B4.model.resolutionrospotrebnadzor.PayFine', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionRospotrebnadzorPayFine'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', useNull: false },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Amount', defaultValue: 0 },
        { name: 'GisUip' },
        { name: 'TypeDocumentPaid', defaultValue: 10 }
    ]
});