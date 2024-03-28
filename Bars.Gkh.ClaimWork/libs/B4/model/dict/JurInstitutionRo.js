Ext.define('B4.model.dict.JurInstitutionRo', {
    extend: 'B4.base.Model',

    fields: [
	    { name: 'Id' },
		{ name: 'Address' },
		{ name: 'Municipality' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'JurInstitution',
        listAction: 'ListRealObjByMunicipality'
    }
});
