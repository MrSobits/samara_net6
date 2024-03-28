Ext.define('B4.store.gisGkh.ROForGisGkhExportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.ROForGisGkhExportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.ROForGisGkhExportModel',
    storeId: 'gisGkhROForGisGkhExportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ROForGisGkhExport'
    }
});