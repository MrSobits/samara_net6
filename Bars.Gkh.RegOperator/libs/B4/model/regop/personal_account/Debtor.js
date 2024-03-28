﻿Ext.define('B4.model.regop.personal_account.Debtor', {
	extend: 'B4.base.Model',

	fields: [
		{ name: 'RoomAddress' },
		{ name: 'Room' },
		{ name: 'State' },
		{ name: 'Municipality' },
		{ name: 'Settlement' },
		{ name: 'AccountOwner' },
		{ name: 'PersonalAccountNum' },
		{ name: 'AreaShare' },
		{ name: 'ChargedSum' },
		{ name: 'PaidSum' },
		{ name: 'PenaltySum' },
		{ name: 'RealityObject' },
		{ name: 'OpenDate' },
		{ name: 'CloseDate' },
		{ name: 'Tariff' },
		{ name: 'RoomNum' },
		{ name: 'Id' },
		{ name: 'OwnerType' },
		{ name: 'ContractNumber' },
		{ name: 'ContractDate' },
		{ name: 'ContractSendDate' },
		{ name: 'ContractDocument' },
		{ name: 'OwnershipDocumentType' },
		{ name: 'DocumentNumber' },
		{ name: 'DocumentRegistrationDate' },
		{ name: 'RoomId' },
		{ name: 'Summary' },
		{ name: 'HasCharges' },
		{ name: 'TotalDebt' },
		{ name: 'ContributionsInArrearsTariff' },
		{ name: 'DebtSum' },
		{ name: 'DebtBaseTariffSum' },
		{ name: 'DebtDecisionTariffSum' },
		{ name: 'ExpirationDaysCount' },
		{ name: 'PenaltyDebt' },
		{ name: 'ExpirationMonthCount' },
		{ name: 'HasClaimWork' },
		{ name: 'CourtType' },
		{ name: 'JurInstitution' },
		{ name: 'UserName' },
		{ name: 'Underage' },
		{ name: 'ClaimworkId' },
		{ name: 'LastClwDebt' },
		{ name: 'PaymentsSum' },
		{ name: 'MewClaimDebt' },
		{ name: 'LastPirPeriod' },
		{ name: 'OwnerArea' },
		{ name: 'RoomArea' },
		{ name: 'ExtractExists', defaultValue: 20 },
		{ name: 'AccountRosregMatched', defaultValue: 20 },
		{ name: 'Separate' },
		{ name: 'ProcessedByTheAgent' }
	],

	proxy: {
		type: 'b4proxy',
		controllerName: 'Debtor'
	}
});
