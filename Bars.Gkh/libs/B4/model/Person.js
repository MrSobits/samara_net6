Ext.define('B4.model.Person', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'person'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Surname' },
        { name: 'Name' },
        { name: 'Patronymic' },
        { name: 'FullName' },
        { name: 'Inn' },
        { name: 'Email' },
        { name: 'Phone' },
        { name: 'AddressReg' },
        { name: 'AddressLive' },
        { name: 'AddressBirth' },
        { name: 'Birthdate' },
        { name: 'TypeIdentityDocument', defaultValue: 10 },
        { name: 'IdSerial' },
        { name: 'IdNumber' },
        { name: 'IdIssuedBy' },
        { name: 'IdIssuedDate' },
        { name: 'ContragentName' },
        { name: 'QcNumber' },
        { name: 'QcIssuedDate' },
        { name: 'QcEndDate' },
        { name: 'State' }
    ]
});