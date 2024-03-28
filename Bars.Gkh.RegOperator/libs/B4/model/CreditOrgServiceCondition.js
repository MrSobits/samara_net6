Ext.define('B4.model.CreditOrgServiceCondition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CreditOrgServiceCondition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CreditOrg', defaultValue: null },
        { name: 'CreditOrgName' },
        { name: 'CashServiceSize', defaultValue: null },
        { name: 'CashServiceDateFrom', defaultValue: null },
        { name: 'CashServiceDateTo', defaultValue: null },
        { name: 'OpeningAccPay', defaultValue: null },
        { name: 'OpeningAccDateFrom', defaultValue: null },
        { name: 'OpeningAccDateTo', defaultValue: null }
    ]
});