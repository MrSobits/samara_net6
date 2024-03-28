Ext.define('B4.store.objectcr.PerformedWorkActForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.PerformedWorkAct'],
    autoLoad: false,
    storeId: 'performedWorkActStore',
    model: 'B4.model.objectcr.PerformedWorkAct',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedWorkAct',
        listAction: 'ListByActiveNewOpenPrograms'
    }
});