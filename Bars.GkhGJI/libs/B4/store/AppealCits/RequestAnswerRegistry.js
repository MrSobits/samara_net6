Ext.define('B4.store.appealcits.RequestAnswerRegistry', {
    extend: 'B4.base.Store',
    requires: ['B4.model.appealcits.RequestAnswer'],
    autoLoad: false,
    storeId: 'appealCitsRequestAnswerRegistryStore',
    model: 'B4.model.appealcits.RequestAnswer'
});