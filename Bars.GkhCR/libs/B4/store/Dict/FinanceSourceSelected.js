Ext.define('B4.store.dict.FinanceSourceSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.FinanceSource'],
    autoLoad: false,
    storeId: 'financeSelectedSource',
    model: 'B4.model.dict.FinanceSource'
});