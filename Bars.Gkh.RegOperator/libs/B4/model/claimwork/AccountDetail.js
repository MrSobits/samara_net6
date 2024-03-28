Ext.define('B4.model.claimwork.AccountDetail', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'AccountId' },
        { name: 'Municipality' },
        { name: 'RoomAddress' },
        { name: 'PersonalAccountNum' },
        { name: 'OwnerName' },
        { name: 'PersAccState' },
        { name: 'CurrPenaltyDebt' },
        { name: 'CurrChargeBaseTariffDebt' },
        { name: 'CurrChargeDecisionTariffDebt' },
        { name: 'CurrChargeDebt' },
        { name: 'CountDaysDelay' },
        { name: 'CountMonthDelay' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'ClaimWorkAccountDetail'
    }
});
