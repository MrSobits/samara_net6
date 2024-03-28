Ext.define('B4.store.gisGkh.ContragentForGisGkhExportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.ContragentForGisGkhExportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.ContragentForGisGkhExportModel',
    storeId: 'gisGkhContragentForGisGkhExportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ContragentForGisGkhExport'
    }
});