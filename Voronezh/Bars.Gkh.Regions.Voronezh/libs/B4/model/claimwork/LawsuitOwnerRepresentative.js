Ext.define('B4.model.claimwork.LawsuitOwnerRepresentative', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'LawsuitOwnerRepresentative'
    },

    fields: [
        { name: 'Id' },
        { name: 'Rloi' },
        { name: 'RepresentativeType' },
        { name: 'Surname' },
        { name: 'FirstName' },
        { name: 'Patronymic' },
        { name: 'BirthDate' },
        { name: 'BirthPlace' },
        { name: 'LivePlace' },
        { name: 'Note' }
    ]
});


