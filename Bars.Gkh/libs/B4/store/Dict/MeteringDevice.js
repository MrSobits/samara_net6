Ext.define('B4.store.dict.MeteringDevice', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.MeteringDevice'],
    autoLoad: false,
    storeId: 'meteringDeviceStore',
    model: 'B4.model.dict.MeteringDevice'
});