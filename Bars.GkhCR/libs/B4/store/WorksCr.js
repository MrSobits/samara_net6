Ext.define('B4.store.WorksCr', {
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.TypeWorkCr'],
    autoLoad: false,
    model: 'B4.model.objectcr.TypeWorkCr',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkCr',
        listAction: 'ListWorksCr'
    }
});