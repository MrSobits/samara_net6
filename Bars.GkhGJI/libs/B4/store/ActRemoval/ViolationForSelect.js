Ext.define('B4.store.actremoval.ViolationForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.actremoval.Violation'],
    autoLoad: false,
    storeId: 'actRemovalViolationForSelectStore',
    model: 'B4.model.actremoval.Violation'
});