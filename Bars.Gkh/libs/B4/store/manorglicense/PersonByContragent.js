Ext.define('B4.store.manorglicense.PersonByContragent', {
    extend: 'B4.base.Store',
    fields: ['Id', 'FullName', 'Position'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseRequest',
        listAction: 'GetListPersonByContragent'
    }
});

