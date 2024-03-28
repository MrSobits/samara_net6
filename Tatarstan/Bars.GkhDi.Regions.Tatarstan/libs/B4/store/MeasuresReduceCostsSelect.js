Ext.define('B4.store.MeasuresReduceCostsSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.MeasuresReduceCosts'],
    autoLoad: false,
    storeId: 'measuresReduceCostsStore',
    model: 'B4.model.dict.MeasuresReduceCosts'
});