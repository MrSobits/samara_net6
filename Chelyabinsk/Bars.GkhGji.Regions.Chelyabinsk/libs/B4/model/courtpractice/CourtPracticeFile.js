Ext.define('B4.model.courtpractice.CourtPracticeFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeFile'
    },
    fields: [
        { name: 'Id' },
        { name: 'CourtPractice'},
        { name: 'FileInfo'},
        { name: 'DocumentName'},
        { name: 'Description' },
        { name: 'DocDate' }
       
    ]
});