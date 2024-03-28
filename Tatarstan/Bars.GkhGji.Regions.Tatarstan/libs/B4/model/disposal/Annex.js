Ext.define('B4.model.disposal.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatDisposalAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Disposal', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'ErknmTypeDocument'}
    ]
});