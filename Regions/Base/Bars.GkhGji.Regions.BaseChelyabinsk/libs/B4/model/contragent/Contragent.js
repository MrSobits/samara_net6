﻿Ext.define('B4.model.contragent.Contragent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.ContragentState', 'B4.enums.GroundsTermination'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'StoreContragent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null, mapping: function (record) { return record && record.Municipality && record.Municipality.Name } },
        { name: 'Parent', defaultValue: null },
        { name: 'OrganizationForm', defaultValue: null },
        { name: 'OrganizationFormName' },
        { name: 'ContragentState', defaultValue: 10 },
        { name: 'AddressOutsideSubject' },
        { name: 'FactAddress' },
        { name: 'MailingAddress' },
        { name: 'JuridicalAddress' },
        { name: 'FiasOutsideSubjectAddress', defaultValue: null },
        { name: 'FiasFactAddress', defaultValue: null },
        { name: 'FiasMailingAddress', defaultValue: null },
        { name: 'FiasJuridicalAddress', defaultValue: null },
        { name: 'Name' },
        { name: 'ShortName' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Okved' },
        { name: 'Okpo' },
        { name: 'DateTermination' },
        { name: 'Description' },
        { name: 'Email' },
        { name: 'IsSite', defaultValue: false },
        { name: 'OfficialWebsite' },
        { name: 'Ogrn' },
        { name: 'OgrnRegistration' },
        { name: 'OrgLegalForm' },
        { name: 'Phone' },
        { name: 'PhoneDispatchService' },
        { name: 'SubscriberBox' },
        { name: 'TweeterAccount' },
        { name: 'YearRegistration' },
        { name: 'DateRegistration' },
        { name: 'ActivityDateStart' },
        { name: 'ActivityDateEnd' },
        { name: 'ActivityDescription' },
        { name: 'ActivityGroundsTermination', defaultValue: 10 },
        { name: 'NameGenitive' },
        { name: 'NameDative' },
        { name: 'NameAccusative' },
        { name: 'NameAblative' },
        { name: 'NamePrepositional' },
        { name: 'Okato' },
        { name: 'Oktmo' },
        { name: 'TimeZoneType' },
        { name: 'Okogu' },
        { name: 'Okfs' },
        { name: 'EgrulExcDate' }
    ]
});