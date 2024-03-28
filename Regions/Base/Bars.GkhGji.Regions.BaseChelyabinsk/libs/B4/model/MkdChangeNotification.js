Ext.define('B4.model.MkdChangeNotification', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MkdChangeNotification'
    },
    fields: [
        { name: 'Id' },
        { name: 'RegistrationNumber' },
        { name: 'Date' },
		{ name: 'RealityObjectFantom' },
		{ name: 'NotificationCause' },
		{ name: 'InboundNumber' },
		{ name: 'RegistrationDate' },
		{ name: 'OldMkdManagementMethod' },
		{ name: 'OldManagingOrganization' },
		{ name: 'OldInn' },
		{ name: 'OldOgrn' },
		{ name: 'NewMkdManagementMethod' },
		{ name: 'NewManagingOrganization' },
		{ name: 'NewInn' },
		{ name: 'NewOgrn' },
		{ name: 'NewJuridicalAddress' },
		{ name: 'NewManager' },
		{ name: 'NewPhone' },
		{ name: 'NewEmail' },
		{ name: 'NewOfficialSite' },
		{ name: 'NewActCopyDate', defaultValue: null},
        { name: 'State' },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'Address' },
        { name: 'FiasAddress' }
    ]
});