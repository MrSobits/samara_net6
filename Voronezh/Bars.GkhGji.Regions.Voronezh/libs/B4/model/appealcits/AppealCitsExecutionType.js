Ext.define('B4.model.appealcits.AppealCitsExecutionType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsExecutionType'
    },
    fields: [
        { name: 'Id' },
        { name: 'AppealCits' },
        { name: 'AppealExecutionType' },
        { name: 'Name' }
    ]
});