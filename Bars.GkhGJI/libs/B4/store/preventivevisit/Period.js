//стор приложений акта проверки
Ext.define('B4.store.preventivevisit.Period', {
    extend: 'B4.base.Store',
    requires: ['B4.model.preventivevisit.Period'],
    autoLoad: false,
    storeId: 'preventivevisitPeriodStore',
    model: 'B4.model.preventivevisit.Period'
});