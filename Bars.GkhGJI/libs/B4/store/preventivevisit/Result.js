//стор приложений акта проверки
Ext.define('B4.store.preventivevisit.Result', {
    extend: 'B4.base.Store',
    requires: ['B4.model.preventivevisit.Result'],
    autoLoad: false,
    storeId: 'preventivevisitResultStore',
    model: 'B4.model.preventivevisit.Result'
});