Ext.define('B4.model.appealcits.TypeOfFeedback', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsTypeOfFeedback'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'FileInfo', defaultValue: null },
        { name: 'TypeOfFeedback', defaultValue: null }
    ]
});