Ext.define('B4.store.InfoAboutReductionPayment', {
    extend: 'B4.base.Store',
    requires: ['B4.model.InfoAboutReductionPayment'],
    autoLoad: false,
    storeId: 'infoAboutReductionPaymentStore',
    groupField: 'TypeGroupServiceDi',
    model: 'B4.model.InfoAboutReductionPayment'
});