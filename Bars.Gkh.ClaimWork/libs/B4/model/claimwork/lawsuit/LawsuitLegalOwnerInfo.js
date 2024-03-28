Ext.define('B4.model.claimwork.lawsuit.LawsuitLegalOwnerInfo', {
    extend: 'B4.model.claimwork.lawsuit.LawsuitOwnerInfo',

    fields: [
		{ name: 'ContragentName' },
		{ name: 'Inn' },
        { name: 'Kpp' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'LawsuitLegalOwnerInfo'
    }
});
