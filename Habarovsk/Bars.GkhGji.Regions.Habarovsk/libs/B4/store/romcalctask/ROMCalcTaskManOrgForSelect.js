Ext.define('B4.store.romcalctask.ROMCalcTaskManOrgForSelect', {
    extend: 'B4.base.Store',
    fields: ['Id', 'ShortName', 'Inn'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseGis',
        listAction: 'ListManOrg'
    }
});
