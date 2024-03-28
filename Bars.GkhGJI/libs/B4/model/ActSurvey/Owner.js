Ext.define('B4.model.actsurvey.Owner', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActSurveyOwner'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActSurvey', defaultValue: null },
        { name: 'Position' },
        { name: 'WorkPlace' },
        { name: 'Fio' },
        { name: 'DocumentName' }
    ]
});