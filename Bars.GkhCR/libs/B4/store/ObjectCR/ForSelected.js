Ext.define('B4.store.objectcr.ForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ObjectCr'],
    autoLoad: false,
    storeId: 'objectCrSelectedStore',
    model: 'B4.model.ObjectCr'
});