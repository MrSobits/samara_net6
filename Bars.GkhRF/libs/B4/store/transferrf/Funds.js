Ext.define('B4.store.transferrf.Funds', {
    extend: 'B4.base.Store',
    requires: ['B4.model.transferrf.Funds'],
    autoLoad: false,
    storeId: 'transferFundsRfStore',
    model: 'B4.model.transferrf.Funds'
});