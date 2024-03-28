Ext.define('B4.store.disposal.ViolationForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.disposal.Violation'],
    autoLoad: false,
    storeId: 'disposalViolationForSelectedStore',
    model: 'B4.model.disposal.Violation'
});