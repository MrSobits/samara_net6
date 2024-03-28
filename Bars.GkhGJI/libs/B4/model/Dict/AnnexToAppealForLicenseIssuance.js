Ext.define('B4.model.dict.AnnexToAppealForLicenseIssuance', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AnnexToAppealForLicenseIssuance'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' }
    ]
});