Ext.define('B4.store.gisGkh.BuildContractImportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.BuildContractImportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.BuildContractImportModel',
    storeId: 'BuildContractImportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'BuildContractImport'
    }
});