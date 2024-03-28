Ext.define('B4.store.dict.KindRiskForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.KindRisk'],
    autoLoad: false,
    storeId: 'kindRiskForSelectedStore',
    model: 'B4.model.dict.KindRisk'
});