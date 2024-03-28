Ext.define('B4.store.basedefault.RealityObject', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObjectGji'],
    autoLoad: false,
    storeId: 'baseDefaultRealityObjectStore',
    model: 'B4.model.RealityObjectGji'
});