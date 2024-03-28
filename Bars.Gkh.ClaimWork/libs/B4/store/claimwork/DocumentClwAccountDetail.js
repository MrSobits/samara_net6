Ext.define('B4.store.claimwork.DocumentClwAccountDetail', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Address' },
        { name: 'AccountNumber' },
        { name: 'DebtBaseTariffSum' },
        { name: 'DebtDecisionTariffSum' },
        { name: 'DebtSum' },
        { name: 'PenaltyDebtSum' },
        { name: 'PenaltyCalcFormula' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentClwAccountDetail'
    }
});