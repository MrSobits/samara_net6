Ext.define('B4.store.dict.LegalReasonForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.LegalReason'],
    autoLoad: false,
    storeId: 'legalReasonSelectStore',
    model: 'B4.model.dict.LegalReason'
});