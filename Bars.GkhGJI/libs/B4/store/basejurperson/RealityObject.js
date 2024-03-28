Ext.define('B4.store.basejurperson.RealityObject', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObjectGji'],
    autoLoad:false,
    storeId: 'baseJurPersonRealityObjectStore',
    model: 'B4.model.RealityObjectGji'
});