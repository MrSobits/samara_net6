Ext.define('B4.model.surveyplan.ContragentAttachment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SurveyPlanContragentAttachment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Date' },
        { name: 'Description' },
        { name: 'File' },
        { name: 'Name' },
        { name: 'Num' },
        { name: 'SurveyPlanContragent' }
    ]
});