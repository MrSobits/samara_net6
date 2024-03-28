Ext.define('B4.model.suggestion.Comment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SuggestionComment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CreationDate', defaultValue: null},
        { name: 'Question' },
        { name: 'Answer' },
        { name: 'AnswerDate' },
        { name: 'CitizenSuggestion' },
        { name: 'HasFiles' },
        { name: 'IsFirst'},
        { name: 'ProblemPlace' },
        { name: 'Description' },
        { name: 'ExecutorType', defaultValue: -1 },
        { name: 'Executor' },
        { name: 'Rubric' },
        { name: 'IsLast'}
    ]
});