Ext.define('B4.model.dict.JurInstitution', {
    extend: 'B4.base.Model',

    fields: [
	    { name: 'Id' },
		{ name: 'JurInstitutionType', defaultValue: 10 },
		{ name: 'CourtType', defaultValue: 10 },
		{ name: 'Municipality' },
		{ name: 'Name' },
	    { name: 'ShortName' },
		{ name: 'FiasAddress' },
		{ name: 'Address' },
        { name: 'PostCode' },
		{ name: 'Phone' },
		{ name: 'OutsideAddress' },
		{ name: 'Email' },
	    { name: 'Website' },
        { name: 'JudgePosition' },
	    { name: 'JudgeSurname' },
	    { name: 'JudgeName' },
	    { name: 'JudgePatronymic' },
	    { name: 'JudgeShortFio' },
		{ name: 'RealObjCount' },
		{ name: 'HeaderText' },
        { name: 'Code' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'JurInstitution'
    }
});
