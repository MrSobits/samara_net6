Ext.define('B4.aspects.permission.tatarstanresolutiongji.FieldRequirement', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.tatarstanresolutiongjifieldrequirement',

    init: function () {
        this.requirements = [
            //реквизиты
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DocumentNumber', applyTo: '[name=DocumentNumber]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DocumentDate', applyTo: '[name=DocumentDate]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DocumentNum', applyTo: '[name=DocumentNum]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.LiteralNum', applyTo: '[name=LiteralNum]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DocumentSubNum', applyTo: '[name=DocumentSubNum]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DeliveryDate', applyTo: '[name=DeliveryDate]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.TypeInitiativeOrg', applyTo: '[name=TypeInitiativeOrg]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.SectorNumber', applyTo: '[name=SectorNumber]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.FineMunicipality', applyTo: '[name=FineMunicipality]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Inspector', applyTo: '[name=Official]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Municipality', applyTo: '[name=Municipality]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Sanction', applyTo: '[name=Sanction]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.PenaltyAmount', applyTo: '[name=PenaltyAmount]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DateTransferSsp', applyTo: '[name=DateTransferSsp]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.TerminationDocumentNum', applyTo: '[name=TerminationDocumentNum]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.SurName', applyTo: '[name=SurName]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.CitizenshipType', applyTo: '[name=CitizenshipType]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Name', applyTo: '[name=Name]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Citizenship', applyTo: '[name=Citizenship]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Patronymic', applyTo: '[name=Patronymic]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.IdentityDocumentType', applyTo: '[name=IdentityDocumentType]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.SerialAndNumberDocument', applyTo: '[name=SerialAndNumberDocument]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.BirthDate', applyTo: '[name=BirthDate]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.IssueDate', applyTo: '[name=IssueDate]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.BirthPlace', applyTo: '[name=BirthPlace]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.IssuingAuthority', applyTo: '[name=IssuingAuthority]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Address', applyTo: '[name=Address]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Company', applyTo: '[name=Company]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.MaritalStatus', applyTo: '[name=MaritalStatus]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.RegistrationAddress', applyTo: '[name=RegistrationAddress]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DependentCount', applyTo: '[name=DependentCount]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Salary', applyTo: '[name=Salary]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DelegateFio', applyTo: '[name=DelegateFio]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.ProcurationNumber', applyTo: '[name=ProcurationNumber]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.ProcurationDate', applyTo: '[name=ProcurationDate]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DelegateCompany', applyTo: '[name=DelegateCompany]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.Citizenship', applyTo: '[name=Citizenship]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.ImprovingFact', applyTo: '[name=ImprovingFact]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.DisimprovingFact', applyTo: '[name=DisimprovingFact]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.RulinFio', applyTo: '[name=RulinFio]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.RulingDate', applyTo: '[name=RulingDate]', selector: 'tatarstanresolutiongjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Requisites.RulingNumber', applyTo: '[name=RulingNumber]', selector: 'tatarstanresolutiongjieditpanel' }
        ];

        this.callParent(arguments);
    }
});