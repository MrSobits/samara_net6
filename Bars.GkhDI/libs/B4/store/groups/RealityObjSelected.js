Ext.define('B4.store.groups.RealityObjSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'realityObjSelectedStore',
    model: 'B4.model.RealityObject'
});