Ext.define('B4.store.appealcits.ForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AppealCits'],
    autoLoad: false,
    storeId: 'appealCitsForSelectedStore',
    model: 'B4.model.AppealCits'
});