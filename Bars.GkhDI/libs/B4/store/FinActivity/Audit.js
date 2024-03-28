Ext.define('B4.store.finactivity.Audit', {
    extend: 'B4.base.Store',
    requires: ['B4.model.finactivity.Audit'],
    autoLoad: false,
    storeId: 'finActivityAuditStore',
    model: 'B4.model.finactivity.Audit'
});