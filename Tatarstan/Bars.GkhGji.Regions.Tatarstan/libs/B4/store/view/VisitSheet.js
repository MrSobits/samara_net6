Ext.define('B4.store.view.VisitSheet', {
    requires: ['B4.model.VisitSheet'],
    extend: 'B4.base.Store',
    autoLoad: false,
    model: 'B4.model.VisitSheet',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VisitSheet',
        listAction: 'ListForRegistry'
    }
});