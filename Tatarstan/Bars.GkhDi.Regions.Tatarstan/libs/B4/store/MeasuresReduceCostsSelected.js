Ext.define('B4.store.MeasuresReduceCostsSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.MeasuresReduceCosts'],
    autoLoad: false,
    storeId: 'measuresReduceCostsStore',
    model: 'B4.model.dict.MeasuresReduceCosts'
});