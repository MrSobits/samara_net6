Ext.define('B4.store.dict.PeriodForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Period'],
    autoLoad: false,
    storeId: 'periodSelectStore',
    model: 'B4.model.dict.Period'
});