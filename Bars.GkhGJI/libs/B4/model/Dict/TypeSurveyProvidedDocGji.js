Ext.define('B4.model.dict.TypeSurveyProvidedDocGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyProvidedDocumentGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeSurvey', defaultValue: null },
        { name: 'ProvidedDocGji', defaultValue: null },
        { name: 'Name'}
    ]
});