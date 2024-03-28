Ext.define('B4.store.BaseDefault', {
    extend: 'B4.base.Store',
    requires: ['B4.model.BaseDefault'],
    autoLoad: false,
    storeId: 'baseDefaultStore',
    model: 'B4.model.BaseDefault'
});