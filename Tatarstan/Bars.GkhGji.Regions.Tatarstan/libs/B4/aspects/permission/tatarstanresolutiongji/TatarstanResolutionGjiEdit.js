Ext.define('B4.aspects.permission.tatarstanresolutiongji.TatarstanResolutionGjiEdit', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.tatarstanresolutiongjieditperm',
    applyByPostfix: true,

    init: function () {
        var me = this;
        me.permissions = [
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Edit', applyTo: 'b4savebutton', selector: 'tatarstanresolutiongjieditpanel',
                applyBy: this.setVisible
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Delete', applyTo: '#btnDelete', selector: 'tatarstanresolutiongjieditpanel',
                applyBy: this.setVisible
            },
            //view
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentNumber_View',
                applyTo: '[name=DocumentNumber]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentDate_View',
                applyTo: '[name=DocumentDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentNum_View',
                applyTo: '[name=DocumentNum]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.LiteralNum_View',
                applyTo: '[name=LiteralNum]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentSubNum_View',
                applyTo: '[name=DocumentSubNum]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DeliveryDate_View',
                applyTo: '[name=DeliveryDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.OffenderWas_View',
                applyTo: '[name=OffenderWas]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TypeInitiativeOrg_View',
                applyTo: '[name=TypeInitiativeOrg]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SectorNumber_View',
                applyTo: '[name=SectorNumber]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.FineMunicipality_View',
                applyTo: '[name=FineMunicipality]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Inspector_View',
                applyTo: '[name=Official]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Municipality_View',
                applyTo: '[name=Municipality]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Sanction_View',
                applyTo: '[name=Sanction]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Paided_View',
                applyTo: '[name=Paided]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.PenaltyAmount_View',
                applyTo: '[name=PenaltyAmount]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DateTransferSsp_View',
                applyTo: '[name=DateTransferSsp]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TerminationBasement_View',
                applyTo: '[name=TerminationBasement]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TerminationDocumentNum_View',
                applyTo: '[name=TerminationDocumentNum]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Executant_View',
                applyTo: '[name=Executant]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Contragent_View',
                applyTo: '[name=Contragent]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Ogrn_View',
                applyTo: '[name=Ogrn]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Inn_View',
                applyTo: '[name=Inn]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Kpp_View',
                applyTo: '[name=Kpp]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SettlementAccount_View',
                applyTo: '[name=SettlementAccount]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BankName_View',
                applyTo: '[name=BankName]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.CorrAccount_View',
                applyTo: '[name=CorrAccount]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Bik_View',
                applyTo: '[name=Bik]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Okpo_View',
                applyTo: '[name=Okpo]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Okonh_View',
                applyTo: '[name=Okonh]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Okved_View',
                applyTo: '[name=Okved]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SurName_View',
                applyTo: '[name=SurName]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.CitizenshipType_View',
                applyTo: '[name=CitizenshipType]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Name_View',
                applyTo: '[name=Name]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Patronymic_View',
                applyTo: '[name=Patronymic]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IdentityDocumentType_View',
                applyTo: '[name=IdentityDocumentType]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SerialAndNumberDocument_View',
                applyTo: '[name=SerialAndNumberDocument]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BirthDate_View',
                applyTo: '[name=BirthDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IssueDate_View',
                applyTo: '[name=IssueDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BirthPlace_View',
                applyTo: '[name=BirthPlace]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IssuingAuthority_View',
                applyTo: '[name=IssuingAuthority]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Address_View',
                applyTo: '[name=Address]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Company_View',
                applyTo: '[name=Company]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.MaritalStatus_View',
                applyTo: '[name=MaritalStatus]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RegistrationAddress_View',
                applyTo: '[name=RegistrationAddress]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DependentCount_View',
                applyTo: '[name=DependentCount]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Salary_View',
                applyTo: '[name=Salary]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ResponsibilityPunishment_View',
                applyTo: '[name=ResponsibilityPunishment]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateFio_View',
                applyTo: '[name=DelegateFio]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ProcurationNumber_View',
                applyTo: '[name=ProcurationNumber]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ProcurationDate_View',
                applyTo: '[name=ProcurationDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateCompany_View',
                applyTo: '[name=DelegateCompany]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateResponsibilityPunishment_View',
                applyTo: '[name=DelegateResponsibilityPunishment]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ImprovingFact_View',
                applyTo: '[name=ImprovingFact]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DisimprovingFact_View',
                applyTo: '[name=DisimprovingFact]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulinFio_View',
                applyTo: '[name=RulinFio]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulingDate_View',
                applyTo: '[name=RulingDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulingNumber_View',
                applyTo: '[name=RulingNumber]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            //edit
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentNumber_Edit',
                applyTo: '[name=DocumentNumber]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentDate_Edit',
                applyTo: '[name=DocumentDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentNum_Edit',
                applyTo: '[name=DocumentNum]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.LiteralNum_Edit',
                applyTo: '[name=LiteralNum]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentSubNum_Edit',
                applyTo: '[name=DocumentSubNum]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DeliveryDate_Edit',
                applyTo: '[name=DeliveryDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.OffenderWas_Edit',
                applyTo: '[name=OffenderWas]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TypeInitiativeOrg_Edit',
                applyTo: '[name=TypeInitiativeOrg]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SectorNumber_Edit',
                applyTo: '[name=SectorNumber]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.FineMunicipality_Edit',
                applyTo: '[name=FineMunicipality]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Inspector_Edit',
                applyTo: '[name=Official]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Municipality_Edit',
                applyTo: '[name=Municipality]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Sanction_Edit',
                applyTo: '[name=Sanction]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Paided_Edit',
                applyTo: '[name=Paided]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.PenaltyAmount_Edit',
                applyTo: '[name=PenaltyAmount]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DateTransferSsp_Edit',
                applyTo: '[name=DateTransferSsp]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TerminationBasement_Edit',
                applyTo: '[name=TerminationBasement]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TerminationDocumentNum_Edit',
                applyTo: '[name=TerminationDocumentNum]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Executant_Edit',
                applyTo: '[name=Executant]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Contragent_Edit',
                applyTo: '[name=Contragent]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SurName_Edit',
                applyTo: '[name=SurName]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.CitizenshipType_Edit',
                applyTo: '[name=CitizenshipType]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Citizenship_Edit',
                applyTo: '[name=Citizenship]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Name_Edit',
                applyTo: '[name=Name]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Patronymic_Edit',
                applyTo: '[name=Patronymic]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IdentityDocumentType_Edit',
                applyTo: '[name=IdentityDocumentType]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SerialAndNumberDocument_Edit',
                applyTo: '[name=SerialAndNumberDocument]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BirthDate_Edit',
                applyTo: '[name=BirthDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IssueDate_Edit',
                applyTo: '[name=IssueDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BirthPlace_Edit',
                applyTo: '[name=BirthPlace]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IssuingAuthority_Edit',
                applyTo: '[name=IssuingAuthority]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Address_Edit',
                applyTo: '[name=Address]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Company_Edit',
                applyTo: '[name=Company]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.MaritalStatus_Edit',
                applyTo: '[name=MaritalStatus]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RegistrationAddress_Edit',
                applyTo: '[name=RegistrationAddress]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DependentCount_Edit',
                applyTo: '[name=DependentCount]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Salary_Edit',
                applyTo: '[name=Salary]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ResponsibilityPunishment_Edit',
                applyTo: '[name=ResponsibilityPunishment]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateFio_Edit',
                applyTo: '[name=DelegateFio]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ProcurationNumber_Edit',
                applyTo: '[name=ProcurationNumber]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ProcurationDate_Edit',
                applyTo: '[name=ProcurationDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateCompany_Edit',
                applyTo: '[name=DelegateCompany]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateResponsibilityPunishment_Edit',
                applyTo: '[name=DelegateResponsibilityPunishment]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ImprovingFact_Edit',
                applyTo: '[name=ImprovingFact]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DisimprovingFact_Edit',
                applyTo: '[name=DisimprovingFact]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulinFio_Edit',
                applyTo: '[name=RulinFio]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulingDate_Edit',
                applyTo: '[name=RulingDate]',
                selector: 'tatarstanresolutiongjieditpanel',
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulingNumber_Edit',
                applyTo: '[name=RulingNumber]',
                selector: 'tatarstanresolutiongjieditpanel',
            }
        ];
        me.callParent(arguments);
    },

    loadPermissions: function () {
        var me = this;
        return B4.Ajax.request({
            url: B4.Url.action('/Permission/GetPermissions'),
            params: {
                permissions: Ext.encode(me.collectPermissions())
            }
        });
    },

    getGrants: function (grants) {
        return grants;
    },

    preDisable: function () {
        //переопределяем для предотвращения установки disabled readOnly полям
    }
});