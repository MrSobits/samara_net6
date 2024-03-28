Ext.define('B4.model.claimwork.lawsuit.LawsuitDocument', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id', useNull: true },
        { name: 'Lawsuit' },
        { name: 'TypeLawsuitDocument', defaultValue: 10 },
        { name: 'CollectDebtFrom'},
        { name: 'Rosp'},
        { name: 'Number' },
        { name: 'Date' },
        { name: 'File' },
        { name: 'Note' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'LawsuitDocument'
    }
});
