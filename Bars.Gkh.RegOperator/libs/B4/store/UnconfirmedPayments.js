Ext.define('B4.store.UnconfirmedPayments', {
    extend: 'B4.base.Store',
    requires: ['B4.model.UnconfirmedPayments'],
    autoLoad: false,
    model: 'B4.model.UnconfirmedPayments'
});