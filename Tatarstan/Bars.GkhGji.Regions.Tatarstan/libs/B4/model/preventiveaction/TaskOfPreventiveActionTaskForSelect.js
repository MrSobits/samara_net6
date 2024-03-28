Ext.define('B4.model.preventiveaction.TaskOfPreventiveActionTaskForSelect',{
    extend: 'B4.model.preventiveaction.TaskOfPreventiveActionTask',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskOfPreventiveActionTask',
        listAction: 'GetAllTasks'
    },
});