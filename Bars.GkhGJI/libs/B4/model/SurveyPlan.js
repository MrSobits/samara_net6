Ext.define('B4.model.SurveyPlan', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'SurveyPlan'
    },
    fields: [
        { name: 'Id' },
        { name: 'State' },
        { name: 'Name' },
        { name: 'PlanJurPerson' }
    ]
});