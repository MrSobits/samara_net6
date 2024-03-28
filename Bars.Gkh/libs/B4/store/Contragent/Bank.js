Ext.define('B4.store.contragent.Bank', {
    extend: 'B4.base.Store',
    requires: ['B4.model.contragent.Bank'],
    autoLoad: false,
    storeId: 'contragentBankStore',
    model: 'B4.model.contragent.Bank'
});