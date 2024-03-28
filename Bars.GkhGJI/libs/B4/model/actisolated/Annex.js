Ext.define('B4.model.actisolated.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolated', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'File', defaultValue: null }
    ]
});