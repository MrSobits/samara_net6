Ext.define('B4.model.actsurvey.Conclusion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActSurveyConclusion'
    },
    fields: [
        { name: 'Id' },
        { name: 'ActSurvey', defaultValue: null },
        { name: 'Official', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'DocNumber' },
        { name: 'DocDate' },
        { name: 'Description' }
    ]
});