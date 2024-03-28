Ext.define('B4.model.LicenseWithHouse', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseGis',
        listAction: 'ListManOrgWithLicenseAndHouse'
      //  timeout: 100000 // 5 минут для сохранения
    },
    fields: [
        { name: 'Id' },
        { name: 'ContragentName' },
        { name: 'Municipality' },
        { name: 'JuridicalAddress' },
        { name: 'KindKND' },
        { name: 'Inn' },
        { name: 'ShortName' }
    ]
});