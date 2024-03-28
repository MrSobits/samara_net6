Ext.define('B4.store.gisGkh.DisposalForGisGkhStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.DisposalForGisGkhModel'],
    autoLoad: false,
    model: 'B4.model.gisGkh.DisposalForGisGkhModel',
    storeId: 'gisGkhDisposalForGisGkhStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'DisposalAndDecisionForGisGkh'
    }
});