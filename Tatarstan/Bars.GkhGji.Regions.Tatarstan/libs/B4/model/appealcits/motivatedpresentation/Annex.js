Ext.define('B4.model.appealcits.motivatedpresentation.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivatedPresentationAppealCitsAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MotivatedPresentation' },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'File' }
    ]
});