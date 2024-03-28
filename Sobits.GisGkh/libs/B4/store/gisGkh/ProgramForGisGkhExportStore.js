Ext.define('B4.store.gisGkh.ProgramForGisGkhExportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.ProgramForGisGkhExportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.ProgramForGisGkhExportModel',
    storeId: 'gisGkhProgramForGisGkhExportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ProgramForGisGkhExport'
    }
});