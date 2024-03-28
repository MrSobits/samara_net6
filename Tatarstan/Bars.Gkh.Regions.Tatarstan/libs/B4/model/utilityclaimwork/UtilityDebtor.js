Ext.define('B4.model.utilityclaimwork.UtilityDebtor', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'State' },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Address' },
        { name: 'OwnerType', defaultValue: 20 },
        { name: 'AccountOwner' },
        { name: 'PenaltyDebt' },
        { name: 'ChargeDebt' },
        { name: 'CountDaysDelay' },
        { name: 'IsDebtPaid' },
        { name: 'DebtPaidDate' },
        { name: 'PersonalAccountNum' },
        { name: 'PersonalAccountState' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'UtilityDebtorClaimWork'
    }
});