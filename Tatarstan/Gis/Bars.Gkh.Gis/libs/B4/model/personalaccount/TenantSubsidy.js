Ext.define('B4.model.personalaccount.TenantSubsidy', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TenantSubsidyRegister',
        listAction: 'ListByApartmentId'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Pss' },
        { name: 'Surname' },
        { name: 'Name' },
        { name: 'Patronymic' },
        { name: 'DateOfBirth', type: 'date' },
        { name: 'ArticleCode' },
        { name: 'Service' },
        { name: 'BankName' },
        { name: 'BeginDate', type: 'date' },
        { name: 'IncomingSaldo' },
        { name: 'AccruedSum' },
        { name: 'AdvancedPayment' },
        { name: 'PaymentSum' },
        { name: 'SmoSum' },
        { name: 'ChangesSum' },
        { name: 'EndDate', type: 'date' }
    ]
});