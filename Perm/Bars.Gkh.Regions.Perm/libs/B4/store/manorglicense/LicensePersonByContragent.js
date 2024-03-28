Ext.define('B4.store.manorglicense.LicensePersonByContragent', {
    extend: 'B4.base.Store',
    fields: ['Id', 'FullName', 'Position'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicense',
        listAction: 'GetListPersonByContragent'
    }
});

