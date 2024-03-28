Ext.define('B4.store.administration.executionaction.ExecutionAction', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.executionaction.ExecutionAction'],
    autoLoad: false,
    model: 'B4.model.administration.executionaction.ExecutionAction',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ExecutionAction',
    },
    pageSize: 100,
});