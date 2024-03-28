Ext.define('B4.store.gisGkh.BuildContractForActImportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.BuildContractImportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.BuildContractImportModel',
    storeId: 'gisGkhBuildContractForActImportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'BuildContractForActImport'
    }
});