Ext.define('B4.store.gisGkh.MOForGisGkhExportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.MOForGisGkhExportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.MOForGisGkhExportModel',
    storeId: 'gisGkhMOForGisGkhExportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'MOForGisGkhExport'
    }
});