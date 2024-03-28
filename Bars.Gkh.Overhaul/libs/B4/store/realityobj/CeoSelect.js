Ext.define('B4.store.realityobj.CeoSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.CommonEstateObject'],
    autoLoad: false,
    model: 'B4.model.CommonEstateObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CommonEstateObject',
        listAction: 'ListForRealObj'
    }
});