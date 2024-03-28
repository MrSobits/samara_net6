Ext.define('B4.model.finactivity.CommunalService', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityCommunalService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'TypeServiceDi', defaultValue: 10 },
        { name: 'Exact', defaultValue: null },
        { name: 'IncomeFromProviding', defaultValue: null },
        { name: 'DebtPopulationStart', defaultValue: null },
        { name: 'DebtPopulationEnd', defaultValue: null },
        { name: 'DebtManOrgCommunalService', defaultValue: null },
        { name: 'PaidByMeteringDevice', defaultValue: null },
        { name: 'PaidByGeneralNeeds', defaultValue: null },
        { name: 'PaymentByClaim', defaultValue: null },
        { name: 'IsInvalid', defaultValue: false }
    ]
});
