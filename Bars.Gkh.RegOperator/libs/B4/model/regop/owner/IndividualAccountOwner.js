Ext.define('B4.model.regop.owner.IndividualAccountOwner', {
    extend: 'B4.model.regop.owner.PersonalAccountOwner',

    fields: [
        { name: 'FirstName' },
        { name: 'Surname' },
        { name: 'SecondName' },
        { name: 'BirthDate' },
        { name: 'BirthPlace' },
        { name: 'IdentityType' },
        { name: 'IdentitySerial' },
        { name: 'IdentityNumber' },
        { name: 'AddressOutsideSubject' },
        { name: 'FiasFactAddress', defaultValue: null },
        { name: 'Email' },
        { name: 'DateDocumentIssuance' },
        { name: 'Gender' },
        { name: 'RegistrationAddress', defaultValue: null },
        { name: 'RegistrationRoom', defaultValue: null },
        { name: 'DocumentIssuededOrg' }
        //     ,
        // { name: 'FactAddrDoc', defaultValue: null }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'IndividualAccountOwner'
    }
});