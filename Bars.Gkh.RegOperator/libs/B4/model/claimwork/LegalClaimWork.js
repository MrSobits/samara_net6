Ext.define('B4.model.claimwork.LegalClaimWork', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'MunicipalityId' },
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
        { name: 'JurisdictionAddress', useNull: true },
        { name: 'IsDebtPaid' },
        { name: 'DebtPaidDate' },

        { name: 'UserName' },
        { name: 'AccountOwnerName' },
        { name: 'Municipality' },
        { name: 'JuridicalAddress' },
        { name: 'FactAddress' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'ContragentState' },
        { name: 'DateTermination' },
        { name: 'OrganizationForm' },
        { name: 'ParentContragentName' },
        { name: 'AccountsNumber' },
        { name: 'StateName' },

        { name: 'OperManagement' },
        { name: 'OperManReason' },
        { name: 'OperManDate' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'LegalClaimWork',
        timeout: 5 * 60 * 1000 // 5 минут
    }
});