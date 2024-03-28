Ext.define('B4.model.regop.personal_account.ChoicesDebtor', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'State' },
        { name: 'CurrChargeDebt' },
        { name: 'IsDebtPaid' },
        { name: 'DocDate' },
        { name: 'DebtSum' },
        { name: 'DocNumber' },
        { name: 'Document' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'DebtorClaimworkRegoperator',
        listAction: 'GetByOwner',
        timeout: 60000
    }
});