Ext.define('B4.store.groups.RealityObjSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'realityObjSelectStore',
    model: 'B4.model.RealityObject'
});