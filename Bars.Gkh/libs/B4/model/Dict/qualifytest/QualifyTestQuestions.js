Ext.define('B4.model.dict.qualifytest.QualifyTestQuestions', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QualifyTestQuestions'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'IsActual', defaultValue: 10 },
        { name: 'Question' },
        { name: 'Code' }
    ]
});