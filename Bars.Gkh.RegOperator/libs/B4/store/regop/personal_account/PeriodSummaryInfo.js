Ext.define('B4.store.regop.personal_account.PeriodSummaryInfo', {
    extend: 'B4.base.Store',
    fields: [
        { name: 'Id' },
        { name: 'PeriodId' },
        { name: 'AccountId' },
        { name: 'SaldoIn' },
        { name: 'SaldoOut' },
        { name: 'SaldoChange' },
        { name: 'PenaltyChange' },
        { name: 'Date' },
        { name: 'ChargedByBaseTariff' },
        { name: 'TariffPayment' },
        { name: 'Recalc' },
        { name: 'PenaltyChange' },
        { name: 'SaldoInFromServ' },
        { name: 'SaldoOutFromServ' },
        { name: 'SaldoChangeFromServ' },
        { name: 'Period' },
        { name: 'CurrTariffDebt' },
        { name: 'OverdueTariffDebt' },
        { name: 'PerformedWorkCharged' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonAccountDetalization',
        listAction: 'ListPeriodSummaryInfo'
    },
    autoLoad: false
});