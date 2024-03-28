Ext.define('B4.store.gisGkh.DownloadGridStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.DownloadGridModel'],
    model: 'B4.model.gisGkh.DownloadGridModel',
    storeId: 'gisGkhDownloadGridStore',
    autoLoad: true,
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ListDownloads'
    }
});