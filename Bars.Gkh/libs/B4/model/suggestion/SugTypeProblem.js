Ext.define('B4.model.suggestion.SugTypeProblem', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SugTypeProblem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RequestTemplate' },
        { name: 'Name' },
        { name: 'ResponceTemplate'},
        { name: 'Rubric'}
    ]
});