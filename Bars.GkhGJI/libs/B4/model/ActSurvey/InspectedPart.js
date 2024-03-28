Ext.define('B4.model.actsurvey.InspectedPart', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActSurveyInspectedPart'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActSurvey', defaultValue: null },
        { name: 'InspectedPart', defaultValue: null },
        { name: 'Character' },
        { name: 'Description' }
    ]
});