﻿Ext.define('B4.model.tatarstanresolutiongji.TatarstanResolutionGji', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanResolutionGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'State', defaultValue: null },
        { name: 'OffenderWas', defaultValue: 30 },
        { name: 'DeliveryDate' },
        { name: 'TypeInitiativeOrg' },
        { name: 'SectorNumber' },
        { name: 'Municipality' },
        { name: 'Official' },
        { name: 'FineMunicipality' },
        { name: 'Sanction' },
        { name: 'Paided', defaultValue: 30 },
        { name: 'PenaltyAmount' },
        { name: 'DateTransferSsp' },
        { name: 'TerminationBasement', defaultValue: 0 },
        { name: 'TerminationDocumentNum' },
        { name: 'Executant' },
        { name: 'SurName' },
        { name: 'Name' },
        { name: 'Patronymic' },
        { name: 'BirthDate' },
        { name: 'BirthPlace' },
        { name: 'Address' },
        { name: 'MaritalStatus' },
        { name: 'DependentCount' },
        { name: 'CitizenshipType', defaultValue: 10 },
        { name: 'Citizenship' },
        { name: 'IdentityDocumentType' },
        { name: 'SerialAndNumberDocument' },
        { name: 'IssueDate' },
        { name: 'IssuingAuthority' },
        { name: 'Company' },
        { name: 'RegistrationAddress' },
        { name: 'Salary' },
        { name: 'ResponsibilityPunishment', defaultValue: false },
        { name: 'Contragent' },
        { name: 'DelegateFio' },
        { name: 'DelegateCompany' },
        { name: 'ProcurationNumber' },
        { name: 'ProcurationDate' },
        { name: 'DelegateResponsibilityPunishment', defaultValue: false },
        { name: 'ImprovingFact' },
        { name: 'DisimprovingFact' },
        { name: 'RulingNumber' },
        { name: 'RulingDate' },
        { name: 'RulinFio' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: 140 },
        { name: 'TypeDocumentGji', defaultValue: 160 },

        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'DocumentYear' },
        { name: 'DocumentNum' },
        { name: 'LiteralNum' },
        { name: 'DocumentSubNum' },

        //контрагент
        { name: 'Ogrn' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'SettlementAccount' },
        { name: 'BankName' },
        { name: 'CorrAccount' },
        { name: 'Bik' },
        { name: 'Okpo' },
        { name: 'Okonh' },
        { name: 'Okved' }
    ]
});