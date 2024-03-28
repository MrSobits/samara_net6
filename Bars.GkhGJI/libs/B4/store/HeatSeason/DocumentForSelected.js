Ext.define('B4.store.heatseason.DocumentForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.heatseason.Document'],
    autoLoad: false,
    storeId: 'heatSeasonDocStore',
    model: 'B4.model.heatseason.Document'
});