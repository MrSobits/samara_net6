Ext.define('B4.model.preventiveaction.task.Objective', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveActionTaskObjective',
        actionName: 'List'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});