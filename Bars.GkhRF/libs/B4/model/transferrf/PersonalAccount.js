Ext.define('B4.model.transferrf.PersonalAccount', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.TypeFinanceGroup'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferFundsRf',
        listAction: 'ListPersonalAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'FinanceGroup' },
        { name: 'Closed' },
        { name: 'Account' },
        { name: 'FinGroupDisplay'}
    ]
});