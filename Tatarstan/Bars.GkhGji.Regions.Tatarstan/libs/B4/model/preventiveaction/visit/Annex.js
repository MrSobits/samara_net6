Ext.define('B4.model.preventiveaction.visit.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VisitSheetAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'VisitSheet' },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'File' }
    ]
});