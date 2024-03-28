Ext.define('B4.store.AuditLogMapForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AuditLogMap'],
    autoLoad: false,
    model: 'B4.model.AuditLogMap'
});