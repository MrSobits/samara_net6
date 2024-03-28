Ext.define('B4.store.service.WorkPprSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.WorkPpr'],
    autoLoad: false,
    storeId: 'workPprSelectedStore',
    model: 'B4.model.dict.WorkPpr'
});