Ext.define('B4.store.gisGkh.PerfWorkActImportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.PerfWorkActImportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.PerfWorkActImportModel',
    storeId: 'gisGkhPerfWorkActImportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'PerfWorkActImport'
    }
});