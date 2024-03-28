Ext.define('B4.store.emergencyobj.Documents', {
    extend: 'B4.base.Store',
    requires: ['B4.model.emergencyobj.Documents'],
    autoLoad: false,
    storeId: 'emergencyObjStore',
    model: 'B4.model.emergencyobj.Documents'
});