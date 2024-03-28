Ext.define('B4.model.dict.JurInstitutionRealObj', {
    extend: 'B4.base.Model',

    fields: [
	    { name: 'Id' },
		{ name: 'JurInstitution' },
		{ name: 'RealityObject' },
		{ name: 'Municipality' },
		{ name: 'Settlement' },
		{ name: 'PlaceName' },
		{ name: 'StreetName' },
		{ name: 'House' },
		{ name: 'Letter' },
		{ name: 'Housing' },
		{ name: 'Building' },
		{ name: 'Address' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'JurInstitutionRealObj'
    }
});
