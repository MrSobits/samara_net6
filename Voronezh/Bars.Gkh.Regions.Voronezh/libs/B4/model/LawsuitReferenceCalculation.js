Ext.define('B4.model.LawsuitReferenceCalculation', {
    extend: 'B4.base.Model',
 
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'LawsuitReferenceCalculation'
    },
    fields: [
        { name: 'Id' },
        { name: 'AccountNumber' },
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