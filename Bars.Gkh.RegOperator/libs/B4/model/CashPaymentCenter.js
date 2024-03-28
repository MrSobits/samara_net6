Ext.define('B4.model.CashPaymentCenter', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CashPaymentCenter'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'Municipality' },
        { name: 'OrganizationForm' },
        { name: 'ContragentState', defaultValue: 10 },
        { name: 'FactAddress' },
        { name: 'MailingAddress' },
        { name: 'JuridicalAddress' },
        { name: 'Name' },
        { name: 'ShortName' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Ogrn' },
        { name: 'Identifier', defaultValue: null },
        { name: 'ConductsAccrual' },
        { name: 'ShowPersonalData' },
        { name: 'Settlement' }

    ]
});