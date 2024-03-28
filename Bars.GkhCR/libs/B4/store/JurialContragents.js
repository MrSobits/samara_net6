Ext.define('B4.store.JurialContragents', {
    extend: 'B4.base.Store',
    requires: ['B4.model.JurialContragent'],
    autoLoad: false,
    storeId: 'jurialContragentsStore',
    model: 'B4.model.JurialContragent',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        listAction: 'ListJurialContragents'
    }
});