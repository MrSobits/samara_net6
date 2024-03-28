Ext.define('B4.model.appealcits.AppealCitsHistory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EntityChangeLogRecord',
        listAction: 'GetAppealHistory'
    },
    fields: [
        { name: 'Id' },
        { name: 'AuditDate' },
        { name: 'OperatorLogin' },
        { name: 'TypeEntityLogging', defaultValue: 1 },
        { name: 'OperationType', defaultValue: 10 },
        { name: 'OperatorName' },
        { name: 'Wording' },
        { name: 'JsonString' }
    ]
});