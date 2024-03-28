Ext.define('B4.model.regoperator.Account', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Number', defaultValue: null },
        { name: 'Address', defaultValue: null },
        { name: 'DateOpen', defaultValue: null },
        { name: 'DateClose', defaultValue: null },
        { name: 'CreditTotal', defaultValue: 0 },
        { name: 'DebetTotal', defaultValue: 0 },
        { name: 'BalanceIn', defaultValue: 0 },
        { name: 'BalanceOut', defaultValue: 0 }
    ]
});