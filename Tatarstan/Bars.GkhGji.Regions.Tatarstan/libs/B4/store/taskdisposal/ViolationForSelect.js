Ext.define('B4.store.disposal.ViolationForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.disposal.Violation'],
    autoLoad: false,
    storeId: 'disposalViolationForSelectStore',
    model: 'B4.model.disposal.Violation'
});