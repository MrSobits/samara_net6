Ext.define('B4.store.WorkActRegister', {
    /*
     *стор актов выполненных работ, натравлен на модель Акт выполненных работ, метод List перенаправлен на ListAct
    */
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.PerformedWorkActRegister'],
    autoLoad: false,
    storeId: 'workActRegisterStore',
    model: 'B4.model.objectcr.PerformedWorkActRegister',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedWorkAct',
        listAction: 'ListAct'
    }
});