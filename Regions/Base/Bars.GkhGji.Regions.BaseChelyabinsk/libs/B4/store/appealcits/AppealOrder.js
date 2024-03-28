Ext.define('B4.store.appealcits.AppealOrder', {
    extend: 'B4.base.Store',
    requires: ['B4.model.appealcits.AppealOrder'],
    autoLoad: false,
    storeId: 'appealOrderStore',
    model: 'B4.model.appealcits.AppealOrder'
});