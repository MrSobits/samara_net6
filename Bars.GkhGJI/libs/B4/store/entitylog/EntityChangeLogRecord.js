Ext.define('B4.store.entitylog.EntityChangeLogRecord', {
    extend: 'B4.base.Store',
    requires: ['B4.model.entitylog.EntityChangeLogRecord'],
    autoLoad:false,
    storeId: 'entityChangeLogRecordStore',
    model: 'B4.model.entitylog.EntityChangeLogRecord'
});