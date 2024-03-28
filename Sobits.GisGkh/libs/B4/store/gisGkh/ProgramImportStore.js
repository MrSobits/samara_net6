Ext.define('B4.store.gisGkh.ProgramImportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.ProgramImportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.ProgramImportModel',
    storeId: 'ProgramImportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ProgramImport'
    }
});