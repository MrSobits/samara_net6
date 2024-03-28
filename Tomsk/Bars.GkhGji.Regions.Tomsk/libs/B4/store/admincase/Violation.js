Ext.define('B4.store.admincase.Violation', {
    extend: 'B4.base.Store',
    requires: ['B4.model.admincase.Violation'],
    autoLoad:false,
    storeId: 'admincaseViolationStore',
    model: 'B4.model.admincase.Violation'
});