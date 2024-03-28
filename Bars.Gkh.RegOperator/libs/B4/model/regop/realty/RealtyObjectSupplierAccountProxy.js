Ext.define('B4.model.regop.realty.RealtyObjectSupplierAccountProxy', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'AccountNum' },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'Saldo' },
        { name: 'LastOperationDate' },
        { name: 'BankAccountNum' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectAccount',
        readAction: 'GetSupplierAccount'
    }
});