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
        { name: 'PaymentDate' },
        { name: 'Description' }
    ]
});