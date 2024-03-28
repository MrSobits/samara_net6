Ext.define('B4.model.StateTransferRule', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StateTransferRule'
    },
    fields: [
        { name: 'Id', type: 'int', useNull: true },
        { name: 'StateTransfer', type: 'auto', defaultValue: null},
        { name: 'Role', type: 'auto', defaultValue: null},
        { name: 'RuleId', type: 'auto' },
        { name: 'RuleName', type: 'string' },
        { name: 'RuleDescription', type: 'string' },
        { name: 'TypeId', type: 'string' },
        { name: 'TypeName', type: 'string' },
        { name: 'NewState', type: 'string' },
        { name: 'CurrentState', type: 'string' },
        { name: 'RoleName', type: 'string' },
        { name: 'Object', type: 'auto' },
        { name: 'Rule', type: 'auto' }
    ]
});