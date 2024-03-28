Ext.define('B4.store.GkuInfo', {
    extend: 'B4.base.Store',
    requires: ['B4.model.GkuInfo'],
    autoLoad: false,
    model: 'B4.model.GkuInfo',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListGkuInfo'
    }
});