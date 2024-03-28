Ext.define('B4.model.DeliveryAgent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.GroundsTermination'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'DeliveryAgent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent'},
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
        { name: 'Settlement' },
        { name: 'DateRegistration' }
    ]
});