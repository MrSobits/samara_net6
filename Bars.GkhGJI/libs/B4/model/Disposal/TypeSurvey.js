Ext.define('B4.model.disposal.TypeSurvey', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalTypeSurvey'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Disposal', defaultValue: null },
        { name: 'TypeSurvey', defaultValue: null }
    ]
});