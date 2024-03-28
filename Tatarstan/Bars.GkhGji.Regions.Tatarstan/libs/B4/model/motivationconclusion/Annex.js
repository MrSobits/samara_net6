Ext.define('B4.model.motivationconclusion.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivationConclusionAnnex'
    },
    fields: [
        { name: 'Id' },
        { name: 'MotivationConclusion' },
        { name: 'Name' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'File' },
    ]
});