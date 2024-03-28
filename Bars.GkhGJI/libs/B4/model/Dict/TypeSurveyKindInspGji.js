Ext.define('B4.model.dict.TypeSurveyKindInspGji', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyKindInspGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeSurvey', defaultValue: null },
        { name: 'KindCheck', defaultValue: null },
        { name: 'Code' },
        { name: 'KindCheckName' }
    ]
});