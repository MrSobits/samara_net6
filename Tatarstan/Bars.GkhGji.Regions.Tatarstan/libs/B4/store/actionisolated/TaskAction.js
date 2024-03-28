Ext.define('B4.store.actionisolated.TaskAction', {
    extend: 'B4.base.Store',
    requires: ['B4.model.actionisolated.TaskAction'],
    autoLoad: false,
    storeId: 'taskactionStore',
    model: 'B4.model.actionisolated.TaskAction'
});