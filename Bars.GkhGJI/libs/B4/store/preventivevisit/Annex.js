//стор приложений акта проверки
Ext.define('B4.store.preventivevisit.Annex', {
    extend: 'B4.base.Store',
    requires: ['B4.model.preventivevisit.Annex'],
    autoLoad: false,
    storeId: 'preventivevisitAnnexStore',
    model: 'B4.model.preventivevisit.Annex'
});