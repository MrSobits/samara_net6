Ext.define('B4.store.ListReissuanceForGisGMP', {
    extend: 'B4.base.Store',
    requires: ['B4.model.licensereissuance.LicenseReissuance'],
    autoLoad: false,
    model: 'B4.model.licensereissuance.LicenseReissuance',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GISGMPExecute',
        listAction: 'GetListReissuance'
    }
});