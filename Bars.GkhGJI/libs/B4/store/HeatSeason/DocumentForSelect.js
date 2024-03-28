//стор выбора документов подготовки к отопительному сезону
//направлен на метод /HeatSeasonDoc/ListView
Ext.define('B4.store.heatseason.DocumentForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.heatseason.Document'],
    autoLoad: false,
    storeId: 'heatSeasonDocStore',
    model: 'B4.model.heatseason.Document',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatSeasonDoc',
        listAction: 'ListView'
    }
});