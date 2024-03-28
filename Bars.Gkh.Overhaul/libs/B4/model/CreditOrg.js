Ext.define('B4.model.CreditOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'CreditOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'FiasAddress', defultValue: null },
        { name: 'Address', defaultValue: null },
        { name: 'AddressOutSubject', defaultValue: null },
        { name: 'IsAddressOut', defaultValue: false },
        { name: 'Parent', defaultValue: null },
        { name: 'IsFilial', defaultValue: false },
        { name: 'Inn', defaultValue: null },
        { name: 'Kpp', defaultValue: null },
        { name: 'Bik', defaultValue: null },
        { name: 'Okpo', defaultValue: null },
        { name: 'CorrAccount', defaultValue: null },
        { name: 'Ogrn', defaultValue: null },
        { name: 'FiasMailingAddress', defaultValue: null },
        { name: 'MailingAddress', defaultValue: null },
        { name: 'MailingAddressOutSubject', defaultValue: null },
        { name: 'IsMailingAddressOut', defaultValue: false },
        { name: 'Oktmo', defaultValue: null }
    ]
});