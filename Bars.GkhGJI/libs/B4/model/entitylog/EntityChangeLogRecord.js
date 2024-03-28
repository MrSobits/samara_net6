Ext.define('B4.model.entitylog.EntityChangeLogRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EntityChangeLogRecord'
    },
    fields: [
        { name: 'Id' },
        { name: 'EntityId' },
        { name: 'AuditDate' },
        { name: 'OldValue' },
        { name: 'NewValue' },
        { name: 'PropertyType' },
        { name: 'PropertyName' },
        { name: 'OperatorLogin' },
        { name: 'TypeEntityLogging', defaultValue: 1 },
        { name: 'OperationType', defaultValue: 10 },
        { name: 'OperatorName' },
        { name: 'OperatorId' },
        { name: 'DocumentValue' }
    ]
});