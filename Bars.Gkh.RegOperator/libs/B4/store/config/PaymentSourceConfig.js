Ext.define('B4.store.config.PaymentSourceConfig', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.config.ExtPaymentSourceConfig'],
    autoLoad: false,
    root: {
        expanded: true
    },
    model: 'B4.model.config.ExtPaymentSourceConfig',
    defaultRootProperty: 'Children'
});