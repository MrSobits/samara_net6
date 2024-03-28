Ext.define('B4.store.ReportHistory', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.ReportHistory'],
    storeId: 'reportHistoryStore',
    model: 'B4.model.ReportHistory'
});