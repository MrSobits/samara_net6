Ext.define('B4.store.preventiveaction.TaskOfPreventiveActionTaskForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.preventiveaction.TaskOfPreventiveActionTaskForSelect'],
    autoLoad: false,
    storeId: 'taskOfPreventiveActionTaskForSelectedStore',
    model: 'B4.model.preventiveaction.TaskOfPreventiveActionTaskForSelect'
});