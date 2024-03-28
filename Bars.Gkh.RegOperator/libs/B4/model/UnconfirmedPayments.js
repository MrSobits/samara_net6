Ext.define('B4.model.UnconfirmedPayments', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'UnconfirmedPayments'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PersonalAccount' },
        { name: 'Sum' },
        { name: 'Guid' },
        { name: 'PaymentDate' },
        { name: 'Description' },
        { name: 'BankBik' },
        { name: 'BankName' },
        { name: 'IsConfirmed' },
        { name: 'File' }
    ]
});