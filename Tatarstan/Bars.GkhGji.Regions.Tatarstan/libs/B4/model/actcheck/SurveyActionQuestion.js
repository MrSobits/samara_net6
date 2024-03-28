Ext.define('B4.model.actcheck.SurveyActionQuestion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SurveyActionQuestion'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SurveyAction' },
        { name: 'Question' },
        { name: 'Answer' }
    ]
});