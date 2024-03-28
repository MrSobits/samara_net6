Ext.define('B4.model.dict.OSP', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OSP'
    },
    fields: [
        { name: 'Municipality', defaultValue: null },
        { name: 'BankAccount' },
        { name: 'KBK' },
        { name: 'Name' },
        { name: 'ShortName' },
        { name: 'CreditOrg' },
        { name: 'OKTMO' },
        { name: 'Town' },
        { name: 'Street' },
    ]
});