Ext.define('B4.store.admincase.ViolationForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.admincase.Violation'],
    autoLoad: false,
    storeId: 'admincaseViolationForSelectedStore',
    model: 'B4.model.admincase.Violation'
});