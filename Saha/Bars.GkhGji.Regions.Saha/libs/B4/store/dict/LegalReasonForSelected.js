Ext.define('B4.store.dict.LegalReasonForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.LegalReason'],
    autoLoad: false,
    storeId: 'legalReasonSelectedStore',
    model: 'B4.model.dict.LegalReason'
});