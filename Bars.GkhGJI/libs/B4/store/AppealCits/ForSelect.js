Ext.define('B4.store.appealcits.ForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AppealCits'],
    autoLoad: false,
    storeId: 'appealCitsForSelectStore',
    model: 'B4.model.AppealCits'
});