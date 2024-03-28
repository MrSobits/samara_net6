Ext.define('B4.store.import.EmbirLog', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Import.Log'],
    autoLoad: false,
    model: 'B4.model.Import.Log',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ImportEmbir',
        listAction: 'ListLog'
    }
});