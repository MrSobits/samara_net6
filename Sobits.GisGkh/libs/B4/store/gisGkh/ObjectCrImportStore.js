Ext.define('B4.store.gisGkh.ObjectCrImportStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.ObjectCrImportModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.ObjectCrImportModel',
    storeId: 'ObjectCrImportStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ObjectCrImport'
    }
});