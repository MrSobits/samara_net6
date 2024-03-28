Ext.define('B4.model.actremoval.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActRemovalAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActRemoval', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'File', defaultValue: null }
    ]
});