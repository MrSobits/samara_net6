Ext.define('B4.store.appealcits.RequestRegistry', {
    extend: 'B4.base.Store',
    requires: ['B4.model.appealcits.Request'],
    autoLoad: false,
    storeId: 'appealCitsRequestRegistryStore',
    model: 'B4.model.appealcits.Request'
});