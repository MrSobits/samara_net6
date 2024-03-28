Ext.define('B4.model.claimwork.lawsuit.LawsuitOwnerInfo', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'OwnerType' },
        { name: 'PersonalAccount' },
        { name: 'StartPeriod' },
        { name: 'EndPeriod' },
        { name: 'CalcPeriod' },
        { name: 'AreaShare' },
        { name: 'AreaShareNumerator' },
        { name: 'AreaShareDenominator' },
        { name: 'DebtBaseTariffSum' },
        { name: 'DebtDecisionTariffSum' },
        { name: 'PenaltyDebt' },
        { name: 'Address' },
        { name: 'Lawsuit' },
        { name: 'SharedOwnership' },
        { name: 'Underage' },
        { name: 'ClaimNumber' },
        { name: 'BirthDate' },
        { name: 'BirthPlace' },
        { name: 'LivePlace' },
        { name: 'JurInstitution' }, 
        { name: 'SNILS' },
        { name: 'DocIndName' },
        { name: 'DocIndSerial' },
        { name: 'DocIndNumber' },
        { name: 'DocIndDate' },
        { name: 'DocIndIssue'}
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'LawsuitOwnerInfo'
    }
});
