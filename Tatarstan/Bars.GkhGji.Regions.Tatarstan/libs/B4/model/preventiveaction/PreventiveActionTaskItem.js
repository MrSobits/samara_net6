Ext.define('B4.model.preventiveaction.PreventiveActionTaskItem',{
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveActionTaskItem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Task'},
        { name: 'Name'}
    ]
});