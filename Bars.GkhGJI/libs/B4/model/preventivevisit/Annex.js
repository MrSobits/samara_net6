Ext.define('B4.model.preventivevisit.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveVisitAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PreventiveVisit', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'TypeAnnex', defaultValue: 0 },
        { name: 'Description' }
    ]
});