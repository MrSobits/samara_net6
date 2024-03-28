Ext.define('B4.model.preventiveaction.visit.InfoProvided', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VisitSheetInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'VisitSheet' },
        { name: 'Info' },
        { name: 'Comment' }
    ]
});