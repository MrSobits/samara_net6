Ext.define('B4.model.appealcits.AppealAnswerExecutionType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealAnswerExecutionType'
    },
    fields: [
        { name: 'Id' },
        { name: 'AppealCitsAnswer' },
        { name: 'AppealExecutionType' },
        { name: 'Name' }
    ]
});