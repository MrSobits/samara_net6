Ext.define('B4.model.administration.executionaction.ExecutionActionTask', {
    extend: 'B4.model.SchedulableTask',
    fields: [
        { name: 'Login', useNull: true },
        { name: 'Name', useNull: true },
        { name: 'Description', useNull: true },
        { name: 'TriggerName', useNull: true },
        { name: 'CreateDate' },
        { name: 'ActionCode' },
        { name: 'BaseParams', useNull: true }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ExecutionActionTask'
    }
});