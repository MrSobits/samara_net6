Ext.define('B4.store.ListLicRequestForGisGMP', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorglicense.Request'],
    autoLoad: false,
    model: 'B4.model.manorglicense.Request',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GISGMPExecute',
        listAction: 'GetListLicRequest'
    }
});