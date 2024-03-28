Ext.define('B4.store.dict.FinanceSourceSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.FinanceSource'],
    autoLoad: false,
    storeId: 'financeSelectSource',
    model: 'B4.model.dict.FinanceSource'
});