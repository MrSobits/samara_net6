Ext.define('B4.model.claimwork.RestructDebt', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'DocumentNumber' },
        { name: 'DocFile' },
        { name: 'PaymentScheduleFile' },
        { name: 'BaseTariffDebtSum' },
        { name: 'DecisionTariffDebtSum' },
        { name: 'DebtSum' },
        { name: 'PenaltyDebtSum' },
        { name: 'RestructSum' },
        { name: 'PercentSum' },
        { name: 'Status' },
        { name: 'Reason' },

        { name: 'DocumentState' },
        { name: 'PaidDebtSum' },
        { name: 'NotPaidDebtSum' },
        { name: 'TerminationDate' },
        { name: 'TerminationNumber' },
        { name: 'TerminationFile' },
        { name: 'TerminationReason' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'RestructDebt'
    }
});
