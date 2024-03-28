Ext.define('B4.store.tasks.TaskEntry', {
    extend: 'B4.base.Store',
    requires: ['B4.model.tasks.TaskEntry'],
    storeId: 'taskEntry',
    model: 'B4.model.tasks.TaskEntry',
    groupField: 'ParentId',
    autoLoad: false
});