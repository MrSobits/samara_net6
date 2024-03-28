Ext.define('B4.store.gisGkh.PremisesGridStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.PremisesGridModel'],
    model: 'B4.model.gisGkh.PremisesGridModel',
    storeId: 'gisGkhPremisesGridStore',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ListPremises'
    }
});