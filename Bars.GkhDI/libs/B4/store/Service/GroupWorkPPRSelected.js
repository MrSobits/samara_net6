Ext.define('B4.store.service.GroupWorkPprSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.GroupWorkPpr'],
    autoLoad: false,
    storeId: 'groupWorkPprSelectedStore',
    model: 'B4.model.dict.GroupWorkPpr'
});