Ext.define('B4.model.preventiveaction.PreventiveActionTaskPlannedAction',{
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveActionTaskPlannedAction'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Action'},
        { name: 'Commentary'},
        { name: 'Task'}
    ]
});