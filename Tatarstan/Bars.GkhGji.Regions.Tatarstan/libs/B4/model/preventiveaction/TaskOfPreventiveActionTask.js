Ext.define('B4.model.preventiveaction.TaskOfPreventiveActionTask',{
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskOfPreventiveActionTask'
    },
    fields: [
        { name: 'Id'},
        { name: 'Name'}
    ]
});