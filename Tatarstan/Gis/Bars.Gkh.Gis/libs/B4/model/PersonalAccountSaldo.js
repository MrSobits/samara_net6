Ext.define('B4.model.PersonalAccountSaldo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountSaldo'
    },
    fields: [
        { name: 'Month', type: 'date' },
        { name: 'PersonalAccountId' },
        { name: 'Service' },
        { name: 'Supplier' },
        { name: 'ServiceId' },
        { name: 'SupplierId' },
        { name: 'IncomingSaldo', type: 'float' },
        { name: 'Credited', type: 'float' },
        { name: 'Recalculation', type: 'float' },
        { name: 'Change', type: 'float' },
        { name: 'Paid', type: 'float' },
        { name: 'OutcomingSaldo', type: 'float' },
        { name: 'Payable', type: 'float' },
        { name: 'Distributed', type: 'float' },
        { name: 'CurrentDebt', type: 'float' }
    ]
});