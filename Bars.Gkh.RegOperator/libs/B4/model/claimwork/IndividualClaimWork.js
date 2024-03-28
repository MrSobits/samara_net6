Ext.define('B4.model.claimwork.IndividualClaimWork', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'AccountOwner' },
        { name: 'DebtorType' },
        { name: 'CurrPenaltyDebt' },
        { name: 'CurrChargeBaseTariffDebt' },
        { name: 'CurrChargeDecisionTariffDebt' },
        { name: 'CurrChargeDebt' },
        { name: 'OrigPenaltyDebt' },
        { name: 'OrigChargeBaseTariffDebt' },
        { name: 'OrigChargeDecisionTariffDebt' },
        { name: 'OrigChargeDebt' },
        { name: 'ClaimWorkTypeBase' },
        { name: 'State' },
        { name: 'DebtorState' },
        { name: 'IsDebtPaid' },
        { name: 'DebtPaidDate' },

        { name: 'UserName' },
        { name: 'AccountOwnerName' },
        { name: 'Municipality' },
        { name: 'RegistrationAddress' },
        { name: 'AccountsAddress' },
        { name: 'AccountsNumber' },
        { name: 'StateName' },

        { name: 'PIRCreateDate' },
        { name: 'FirstDocCreateDate' },
        { name: 'SubContractNum' },
        { name: 'SubContractDate' },
        { name: 'SubContragent' },

        { name: 'HasCharges185FZ' },
        { name: 'MinShare' },
        { name: 'Underage' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'IndividualClaimWork',
        timeout: 5 * 60 * 1000 // 5 минут
    }
});
