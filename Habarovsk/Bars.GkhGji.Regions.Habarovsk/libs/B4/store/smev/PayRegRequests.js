Ext.define('B4.store.smev.PayRegRequests', {
    extend: 'B4.base.Store',
    requires: ['B4.model.smev.PayRegRequests'],
    autoLoad: false,
    storeId: 'payRegRequestsStore',
    model: 'B4.model.smev.PayRegRequests'
});