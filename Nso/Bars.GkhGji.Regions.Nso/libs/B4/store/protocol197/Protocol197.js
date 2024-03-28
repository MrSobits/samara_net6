Ext.define('B4.store.protocol197.Protocol197', {
    extend: 'B4.base.Store',
    model: 'B4.model.protocol197.Protocol197',
    requires: ['B4.model.protocol197.Protocol197'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol197',
        listAction: 'ListView',
        timeout: 300000
    }
});