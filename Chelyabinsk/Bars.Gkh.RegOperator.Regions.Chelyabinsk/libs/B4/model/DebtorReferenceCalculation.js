Ext.define('B4.model.DebtorReferenceCalculation', {
    extend: 'B4.base.Model',


    proxy: {
        type: 'b4proxy',
        controllerName: 'DebtorReferenceCalculation'
    },
    fields: [
        { name: 'Id' },
        { name: 'AccountNumber' },
        { name: 'AgentPIRDebtor' },
        { name: 'Name' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'AreaShare' },
        { name: 'BaseTariff' },
        { name: 'RoomArea' },
        { name: 'TarifPayment' },
        { name: 'TariffCharged' },
        { name: 'TarifDebt' },
        { name: 'TarifDebtPay', defaultValue: 0 },
        { name: 'PaymentDate' },
        { name: 'Description' },
        { name: 'Penalties' },
        { name: 'AccrualPenalties' },
        { name: 'AccrualPenaltiesFormula' },
        { name: 'PenaltyPayment' },
        { name: 'PenaltyPaymentDate' }
    ]
});