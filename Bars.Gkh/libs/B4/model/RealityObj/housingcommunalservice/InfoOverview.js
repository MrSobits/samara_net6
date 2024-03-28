Ext.define('B4.model.realityobj.housingcommunalservice.InfoOverview', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseInfoOverview'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'IndividualAccountsCount' },
        { name: 'IndividualTenantAccountsCount' },
        { name: 'IndividualOwnerAccountsCount' },
        { name: 'LegalAccountsCount' },
        { name: 'LegalTenantAccountsCount' },
        { name: 'LegalOwnerAccountsCount' }
    ]
});