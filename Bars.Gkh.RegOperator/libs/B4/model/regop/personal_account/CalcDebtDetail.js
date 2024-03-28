Ext.define('B4.model.regop.personal_account.CalcDebtDetail', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CalcDebtDetail'
    },

    fields: [
        { name: 'Id' },
        { name: 'CalcDebt' },
        { name: 'OwnerId' },
        { name: 'AccountOwner' },
        { name: 'AccountOwnerName' },
        { name: 'OwnerType' },
        { name: 'Type' },
        { name: 'AreaShare' },
        { name: 'ChargeBaseTariff' },
        { name: 'ChargeDecTariff' },
        { name: 'ChargePenalty' },
        { name: 'PaymentBaseTariff' },
        { name: 'PaymentDecTariff' },
        { name: 'PaymentPenalty' },
        { name: 'DistributionDebtBaseTariff' },
        { name: 'DistributionDebtDecTariff' },
        { name: 'DistributionDebtPenalty' },
        { name: 'DistributionPayBaseTariff' },
        { name: 'DistributionPayDecTariff' },
        { name: 'DistributionPayPenalty' },
        { name: 'SaldoOutBaseTariff' },
        { name: 'SaldoOutDecisionTariff' },
        { name: 'SaldoOutPenalty' }
    ]
});