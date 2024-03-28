//стор приложений акта проверки
Ext.define('B4.store.actcheck.Annex', {
    extend: 'B4.base.Store',
    requires: ['B4.model.actcheck.Annex'],
    autoLoad: false,
    storeId: 'actCheckAnnexStore',
    model: 'B4.model.actcheck.Annex'
});