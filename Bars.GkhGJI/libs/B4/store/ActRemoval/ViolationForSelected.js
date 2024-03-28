Ext.define('B4.store.actremoval.ViolationForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.actremoval.Violation'],
    autoLoad: false,
    storeId: 'actRemovalViolationForSelectedStore',
    model: 'B4.model.actremoval.Violation'
});