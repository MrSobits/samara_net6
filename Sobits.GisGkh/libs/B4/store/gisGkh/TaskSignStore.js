Ext.define('B4.store.gisGkh.TaskSignStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.TaskGridModel'],
    model: 'B4.model.gisGkh.TaskGridModel',
    storeId: 'gisGkhTaskGridStore',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ListTasksForMassSign'
    }
});