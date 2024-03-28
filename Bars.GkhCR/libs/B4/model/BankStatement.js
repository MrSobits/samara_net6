Ext.define('B4.model.BankStatement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BankStatement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'ObjectCrName' },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'ManagingOrganizationName' },
        { name: 'Contragent', defaultValue: null },
        { name: 'ContragentName' },
        { name: 'Period', defaultValue: null },
        { name: 'PeriodName' },
        { name: 'TypeFinanceGroup', defaultValue: 10 },
        { name: 'BudgetYear', defaultValue: null },
        { name: 'IncomingBalance', defaultValue: null },
        { name: 'OutgoingBalance', defaultValue: null },
        { name: 'PersonalAccount'},
        { name: 'DocumentNum' },
        { name: 'OperLastDate', defaultValue: null },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'MunicipalityName' }
    ]
});