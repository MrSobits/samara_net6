Ext.define('B4.store.service.GroupWorkPprSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.GroupWorkPpr'],
    autoLoad: false,
    storeId: 'groupWorkPprSelectStore',
    model: 'B4.model.dict.GroupWorkPpr'
});