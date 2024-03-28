Ext.define('B4.model.competition.Document', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CompetitionDocument'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Competition', defaultValue: null },
        { name: 'DocumentName', defaultValue: null },
        { name: 'DocumentNumber', defaultValue: null },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'File', defaultValue: null }
    ]
});