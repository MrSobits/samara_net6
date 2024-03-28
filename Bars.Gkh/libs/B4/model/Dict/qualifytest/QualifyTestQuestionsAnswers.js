Ext.define('B4.model.dict.qualifytest.QualifyTestQuestionsAnswers', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QualifyTestQuestionsAnswers'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Answer' },
        { name: 'IsCorrect', defaultValue: 20 },
        { name: 'QualifyTestQuestions' }
    ]
});