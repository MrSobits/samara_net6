Ext.define('B4.model.dict.SurveySubject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SurveySubject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Formulation' },
        { name: 'Relevance' },
        { name: 'GisGkhCode' },
        { name: 'GisGkhGuid' }
    ]
});