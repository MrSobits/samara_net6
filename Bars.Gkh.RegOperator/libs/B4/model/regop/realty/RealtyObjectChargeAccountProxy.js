Ext.define('B4.model.regop.realty.RealtyObjectChargeAccountProxy', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'AccountNum' },
        { name: 'DateOpen' },
        { name: 'BankAccountNumber' },
        { name: 'ChargeTotal' },
        { name: 'PaidTotal' },
        { name: 'LastOperationDate' }
    ],
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectAccount',
        readAction: 'GetChargeAccount'
    }
});