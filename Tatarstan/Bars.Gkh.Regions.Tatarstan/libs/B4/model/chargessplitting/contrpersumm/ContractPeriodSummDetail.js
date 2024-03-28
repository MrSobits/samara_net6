Ext.define('B4.model.chargessplitting.contrpersumm.ContractPeriodSummDetail', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractPeriodSummDetail',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' },

        { name: 'ChargedManOrg' },
        { name: 'PaidManOrg' },
        { name: 'SaldoOut' },
        { name: 'BilledPubServOrg' },
        { name: 'PaidPubServOrg' },

        { name: 'StartDebt' },
        { name: 'ChargedResidents' },
        { name: 'RecalcPrevPeriod' },
        { name: 'ChangeSum' },
        { name: 'NoDeliverySum' },
        { name: 'PaidResidents' },
        { name: 'EndDebt' },
        { name: 'ChargedToPay' },
        { name: 'TransferredPubServOrg' }
    ]
});