Ext.define('B4.model.suggestion.Comment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SuggestionComment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CreationDate' },
        { name: 'Question' },
        { name: 'Answer' },
        { name: 'AnswerDate' },
        { name: 'CitizenSuggestion' }
    ]
});