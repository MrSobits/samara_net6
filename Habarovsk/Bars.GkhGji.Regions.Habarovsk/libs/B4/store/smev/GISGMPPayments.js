Ext.define('B4.store.smev.GISGMPPayments', {
    extend: 'B4.base.Store',
    requires: ['B4.model.smev.GISGMPPayments'],
    autoLoad: false,
    storeId: 'gISGMPPaymentsStore',
    model: 'B4.model.smev.GISGMPPayments'
});