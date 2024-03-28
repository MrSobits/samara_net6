Ext.define('B4.store.admincase.ViolationForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.admincase.Violation'],
    autoLoad: false,
    storeId: 'admincaseViolationForSelectStore',
    model: 'B4.model.admincase.Violation'
});