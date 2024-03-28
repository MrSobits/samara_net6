Ext.define('B4.model.contragent.Bank', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContragentBankCreditOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'Name' },
        { name: 'Bik' },
        { name: 'Okonh' },
        { name: 'Okpo' },
        { name: 'CorrAccount' },
        { name: 'SettlementAccount' },
        { name: 'Description' },
        { name: 'CreditOrg', defaultValue: null },
        { name: 'File', defaultValue: null }
    ]
});