Ext.define('B4.model.actsurvey.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActSurveyRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'ActSurvey', defaultValue: null },
        { name: 'RealityObject', defaultValue: null }
    ]
});