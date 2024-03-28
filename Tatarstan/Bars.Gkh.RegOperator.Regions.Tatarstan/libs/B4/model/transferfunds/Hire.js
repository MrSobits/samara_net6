Ext.define('B4.model.transferfunds.Hire', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferHire',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' },
        { name: 'AccountNum' },
        { name: 'Transferred', defaultValue: false },
        { name: 'TransferredSum' },
        { name: 'Municipality' },
        { name: 'PaidTotal' },
        { name: 'BeforeTransfer' }
    ]
});