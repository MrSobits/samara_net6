Ext.define('B4.store.servorg.Service', {
    extend: 'B4.base.Store',
    requires: ['B4.model.servorg.Service'],
    autoLoad: false,
    storeId: 'servorgServiceStore',
    model: 'B4.model.servorg.Service'
});