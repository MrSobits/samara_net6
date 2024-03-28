Ext.define('B4.store.manorglicense.ListManOrg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Contragent'],
    autoLoad: false,
    model: 'B4.model.Contragent',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseRequest',
        listAction: 'ListManOrgForRequestType'
    }
});