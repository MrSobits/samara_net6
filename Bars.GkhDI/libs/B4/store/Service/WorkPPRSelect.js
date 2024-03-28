Ext.define('B4.store.service.WorkPprSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.WorkPpr'],
    autoLoad: false,
    storeId: 'workPprSelectStore',
    model: 'B4.model.dict.WorkPpr'
});