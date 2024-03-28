Ext.define('B4.store.licensereissuance.PersonByContragent', {
    extend: 'B4.base.Store',
    fields: ['Id', 'FullName', 'Position'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseGis',
        listAction: 'GetListPersonByContragentId'
    }
});

