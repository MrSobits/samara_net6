﻿Ext.define('B4.model.claimwork.lawsuit.CourtOrderClaim', {
	extend: 'B4.base.Model',

	fields: [
		{ name: 'Id' },
		{ name: 'ClaimWork' },
		{ name: 'ClaimWorkTypeBase' },
		{ name: 'DocumentType' },
		{ name: 'DocumentDate' },
		{ name: 'DocumentNumber' },
		{ name: 'DocumentNum' },
		{ name: 'State' },
		{ name: 'ConsiderationDate' },
		{ name: 'ConsiderationNumber' },
		{ name: 'WhoConsidered', defaultValue: 0 },
		{ name: 'ResultConsideration', defaultValue: 0 },
		{ name: 'LawsuitDocType', defaultValue: 0 },
		{ name: 'DateEnd' },
		{ name: 'BidDate' },
		{ name: 'BidNumber' },
		{ name: 'File' },
		{ name: 'DebtBaseTariffSum' },
		{ name: 'DebtDecisionTariffSum' },
		{ name: 'DebtSum' },
		{ name: 'PenaltyDebt' },
		{ name: 'Duty' },
		{ name: 'Costs' },
		{ name: 'JurInstitution' },
		{ name: 'JuridicalSectorMu' },
		{ name: 'DateOfAdoption' },
		{ name: 'DateOfRewiew' },
		{ name: 'DebtSumApproved' },
		{ name: 'Suspended' },
		{ name: 'DeterminationNumber' },
		{ name: 'DeterminationDate' },
		{ name: 'PenaltyDebtApproved' },
		{ name: 'CbSize', defaultValue: 0 },
		{ name: 'CbDebtSum' },
		{ name: 'CbPenaltyDebt' },
		{ name: 'CbFactInitiated', defaultValue: 0 },
		{ name: 'CbDateInitiated' },
		{ name: 'CbStationSsp' },
		{ name: 'CbDocumentType', defaultValue: 0 },
		{ name: 'CbSumRepayment' },
		{ name: 'CbDateDocument' },
		{ name: 'CbNumberDocument' },
		{ name: 'CbFile' },
		{ name: 'CbSumStep' },
		{ name: 'CbIsStopped' },
		{ name: 'CbDateStopped' },
		{ name: 'CbReasonStoppedType', defaultValue: 0 },
		{ name: 'CbReasonStoppedDescription' },
		{ name: 'PetitionType', defaultValue: null },
		{ name: 'BaseInfo' },
		{ name: 'Municipality' },
		{ name: 'Address' },
		{ name: 'ObjectionArrived', defaultValue: 10 },
		{ name: 'ClaimDate' },
		{ name: 'Document' },
		{ name: 'DutyPostponement', defaultValue: true },
		{ name: 'DebtStartDate' },
		{ name: 'DebtEndDate' },
		{ name: 'IsLimitationOfActions', defaultValue: false },
		{ name: 'DateLimitationOfActions' },
        { name: 'Description' },
		{ name: 'JudgeName' },
		{ name: 'NumberCourtBuisness' },
		{ name: 'IsDeterminationReturn' },
		{ name: 'DateDeterminationReturn' },
		{ name: 'IsDeterminationRenouncement' },
		{ name: 'DateDeterminationRenouncement' },
		{ name: 'DateJudicalOrder' },
		{ name: 'IsDeterminationCancel' },
		{ name: 'DateDeterminationCancel' },
		{ name: 'IsDeterminationOfTurning' },
		{ name: 'DateDeterminationOfTurning' },
        { name: 'FKRAmountCollected' },
        { name: 'ComentСonsideration' },
        { name: 'PayDocNumber' },
        { name: 'PayDocDate' },
        { name: 'MoneyLess' },
        { name: 'DutyPayed' },
		{name : 'RedirectDate'},
		{name: 'LawsuitDistanceDecisionCancelComment'},
		{name: 'DutyDebtApproved'},
		{name: 'IsDenied'},
		{name: 'DeniedDate'},
		{name: 'IsDeniedAdmission'},
		{name: 'DeniedAdmissionDate'},
		{name: 'IsDirectedByJuridiction'},
		{name: 'DirectedByJuridictionDate'},
		{name: 'DirectedToDebtor'}
	],

	proxy: {
		type: 'b4proxy',
		controllerName: 'CourtOrderClaim'
	}
});