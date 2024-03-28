Ext.define('B4.store.dict.PeriodForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Period'],
    autoLoad: false,
    storeId: 'periodSelectedStore',
    model: 'B4.model.dict.Period'
});