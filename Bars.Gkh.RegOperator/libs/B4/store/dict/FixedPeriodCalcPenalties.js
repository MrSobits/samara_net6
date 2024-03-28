Ext.define('B4.store.dict.FixedPeriodCalcPenalties', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.FixedPeriodCalcPenalties'],
    autoLoad: false,
    storeId: 'penaltiesDeferredStore',
    model: 'B4.model.dict.FixedPeriodCalcPenalties'
});