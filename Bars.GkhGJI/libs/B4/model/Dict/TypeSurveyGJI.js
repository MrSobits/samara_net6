Ext.define('B4.model.dict.TypeSurveyGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'NameGenitive' },
        { name: 'NameDative' },
        { name: 'NameAccusative' },
        { name: 'NameAblative' },
        { name: 'NamePrepositional' }
    ]
});