Ext.define('B4.model.actsurvey.Photo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActSurveyPhoto'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActSurvey', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'IsPrint' },
        { name: 'ImageDate' },
        { name: 'Group', defaultValue: 10 },
        { name: 'Name' },
        { name: 'Description' }
    ]
});