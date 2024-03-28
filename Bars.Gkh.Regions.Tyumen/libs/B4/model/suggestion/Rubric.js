Ext.define('B4.model.suggestion.Rubric', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.ExecutorType'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'rubric'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'FirstExecutorType', defaultValue: 0 },
        { name: 'isActual' },
        { name: 'ExpireSuggestionTerm' }
    ]
});