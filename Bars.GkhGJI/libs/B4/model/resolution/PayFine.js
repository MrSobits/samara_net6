Ext.define('B4.model.resolution.PayFine', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionPayFine'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Amount', defaultValue: 0 },
        { name: 'GisUip' },
        { name: 'TypeDocumentPaid', defaultValue: 10 }
    ]
});