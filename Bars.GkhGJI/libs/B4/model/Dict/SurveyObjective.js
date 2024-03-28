Ext.define('B4.model.dict.SurveyObjective', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SurveyObjective'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' }
    ]
});