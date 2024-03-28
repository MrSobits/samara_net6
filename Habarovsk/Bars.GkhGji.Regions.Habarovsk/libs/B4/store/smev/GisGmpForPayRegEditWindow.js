Ext.define('B4.store.smev.GisGmpForPayRegEditWindow', {
    extend: 'B4.base.Store',
    requires: ['B4.model.smev.GisGmp'],
    autoLoad: false,
    model: 'B4.model.smev.GisGmp',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGmpExecute',
        listAction: 'GisGmpForPayRegEditWindow'
    }
});