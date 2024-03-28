Ext.define('B4.store.basestatement.RealityObject', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObjectGji'],
    autoLoad: false,
    storeId: 'baseStatementRealityObjectStore',
    model: 'B4.model.RealityObjectGji'
});