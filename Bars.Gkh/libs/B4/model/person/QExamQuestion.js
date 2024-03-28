Ext.define('B4.model.person.QExamQuestion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QExamQuestion'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PersonRequestToExam' },
        { name: 'QualifyTestQuestions' },
        { name: 'Number' },
        { name: 'QuestionText' },
        { name: 'QualifyTestQuestionsAnswers' }       
    ]
});