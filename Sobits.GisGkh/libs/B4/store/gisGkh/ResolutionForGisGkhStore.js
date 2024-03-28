Ext.define('B4.store.gisGkh.ResolutionForGisGkhStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.ResolutionForGisGkhModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.ResolutionForGisGkhModel',
    storeId: 'gisGkhResolutionForGisGkhStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ResolutionForGisGkh'
    }
});