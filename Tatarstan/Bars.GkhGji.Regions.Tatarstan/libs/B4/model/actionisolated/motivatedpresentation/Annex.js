Ext.define('B4.model.actionisolated.motivatedpresentation.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivatedPresentationAnnex'
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