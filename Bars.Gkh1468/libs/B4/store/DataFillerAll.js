Ext.define('B4.store.DataFillerAll', {
    extend: 'B4.base.Store',
    requires: ['B4.model.DataFiller'],
    model: 'B4.model.DataFiller',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'DataFiller',
        listAction: 'ListAll'
    }
});