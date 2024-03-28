Ext.define('B4.model.claimwork.LawSuitDebtWorkSSPDocument', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'LawSuitDebtWorkSSPDocument'
    },

    fields: [
        { name: 'Id', useNull: true },
        { name: 'LawSuitDebtWorkSSP' },
        { name: 'TypeLawsuitDocument', defaultValue: 10 },
        { name: 'CollectDebtFrom' },
        { name: 'Rosp' },
        { name: 'Number' },
        { name: 'NumberString' },
        { name: 'Date' },
        { name: 'File' },
        { name: 'Note' }
    ]
});
