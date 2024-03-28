Ext.define('B4.model.preventiveaction.PreventiveActionTaskConsultingQuestion',{
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveActionTaskConsultingQuestion'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Task'},
        { name: 'Question'},
        { name: 'Answer'},
        { name: 'ControlledPerson' }
    ]
});