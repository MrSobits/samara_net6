Ext.define('B4.model.chargessplitting.contrpersumm.ContractPeriodSumm', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PubServContractPeriodSumm',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Period' },
        { name: 'Municipality' },
        { name: 'UoSummId' },
        { name: 'RsoSummId' },
        { name: 'UoState' },
        { name: 'RsoState' },
        { name: 'ManagingOrganization' },
        { name: 'PublicServiceOrg' },
        { name: 'Service' },
        { name: 'ChargedManOrg' },
        { name: 'PaidManOrg' },
        { name: 'SaldoOut' },
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