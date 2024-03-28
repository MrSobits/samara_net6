Ext.define('B4.model.claimwork.lawsuit.LawsuitIndividualOwnerInfo', {
    extend: 'B4.model.claimwork.lawsuit.LawsuitOwnerInfo',

    fields: [
		{ name: 'Surname' },
		{ name: 'FirstName' },
        { name: 'SecondName' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'LawsuitIndividualOwnerInfo'
    }
});
