Ext.define('B4.aspects.permission.tatarstanprotocolgji.TatarstanProtocolGjiEdit', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.tatarstanprotocolgjieditstateperm',
    applyByPostfix: true,

    init: function () {
        var me = this;
        me.permissions = [
            //view
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DocumentNumber_View',
                applyTo: '[name=DocumentNumber]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DocumentDate_View',
                applyTo: '[name=DocumentDate]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Municipality_View',
                applyTo: '[name=Municipality]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateOffense_View',
                applyTo: '[name=DateOffense]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ZonalInspection_View',
                applyTo: '[name=ZonalInspection]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TimeOffense_View',
                applyTo: '[name=TimeOffense]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CheckInspectors_View',
                applyTo: '[name=CheckInspectors]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateSupply_View',
                applyTo: '[name=DateSupply]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Pattern_View',
                applyTo: '[name=Pattern]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.GisUin_View',
                applyTo: '[name=GisUin]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.AnnulReason_View',
                applyTo: '[name=AnnulReason]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.UpdateReason_View',
                applyTo: '[name=UpdateReason]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Sanction_View',
                applyTo: '[name=Sanction]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Paided_View',
                applyTo: '[name=Paided]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.PenaltyAmount_View',
                applyTo: '[name=PenaltyAmount]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateTransferSsp_View',
                applyTo: '[name=DateTransferSsp]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TerminationBasement_View',
                applyTo: '[name=TerminationBasement]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TerminationDocumentNum_View',
                applyTo: '[name=TerminationDocumentNum]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Executant_View',
                applyTo: '[name=Executant]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Contragent_View',
                applyTo: '[name=Contragent]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Ogrn_View',
                applyTo: '[name=Ogrn]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Inn_View',
                applyTo: '[name=Inn]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Kpp_View',
                applyTo: '[name=Kpp]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SettlementAccount_View',
                applyTo: '[name=SettlementAccount]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BankName_View',
                applyTo: '[name=BankName]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CorrAccount_View',
                applyTo: '[name=CorrAccount]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Bik_View',
                applyTo: '[name=Bik]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Okpo_View',
                applyTo: '[name=Okpo]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Okonh_View',
                applyTo: '[name=Okonh]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Okved_View',
                applyTo: '[name=Okved]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SurName_View',
                applyTo: '[name=SurName]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CitizenshipType_View',
                applyTo: '[name=CitizenshipType]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Name_View',
                applyTo: '[name=Name]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Patronymic_View',
                applyTo: '[name=Patronymic]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IdentityDocumentType_View',
                applyTo: '[name=IdentityDocumentType]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SerialAndNumberDocument_View',
                applyTo: '[name=SerialAndNumberDocument]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BirthDate_View',
                applyTo: '[name=BirthDate]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IssueDate_View',
                applyTo: '[name=IssueDate]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BirthPlace_View',
                applyTo: '[name=BirthPlace]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IssuingAuthority_View',
                applyTo: '[name=IssuingAuthority]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Address_View',
                applyTo: '[name=Address]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Company_View',
                applyTo: '[name=Company]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.MaritalStatus_View',
                applyTo: '[name=MaritalStatus]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.RegistrationAddress_View',
                applyTo: '[name=RegistrationAddress]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DependentCount_View',
                applyTo: '[name=DependentCount]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Salary_View',
                applyTo: '[name=Salary]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ResponsibilityPunishment_View',
                applyTo: '[name=ResponsibilityPunishment]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateFio_View',
                applyTo: '[name=DelegateFio]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProcurationNumber_View',
                applyTo: '[name=ProcurationNumber]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProcurationDate_View',
                applyTo: '[name=ProcurationDate]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateCompany_View',
                applyTo: '[name=DelegateCompany]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateResponsibilityPunishment_View',
                applyTo: '[name=DelegateResponsibilityPunishment]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProtocolExplanation_View',
                applyTo: '[name=ProtocolExplanation]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.AccusedExplanation_View',
                applyTo: '[name=AccusedExplanation]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IsInTribunal_View',
                applyTo: '[name=IsInTribunal]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.RejectionSignature_View',
                applyTo: '[name=RejectionSignature]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ResidencePetition_View',
                applyTo: '[name=ResidencePetition]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            //edit
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DocumentNumber_Edit',
                applyTo: '[name=DocumentNumber]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DocumentDate_Edit',
                applyTo: '[name=DocumentDate]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Municipality_Edit',
                applyTo: '[name=Municipality]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateOffense_Edit',
                applyTo: '[name=DateOffense]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ZonalInspection_Edit',
                applyTo: '[name=ZonalInspection]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TimeOffense_Edit',
                applyTo: '[name=TimeOffense]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CheckInspectors_Edit',
                applyTo: '[name=CheckInspectors]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateSupply_Edit',
                applyTo: '[name=DateSupply]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Pattern_Edit',
                applyTo: '[name=Pattern]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.AnnulReason_Edit',
                applyTo: '[name=AnnulReason]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.UpdateReason_Edit',
                applyTo: '[name=UpdateReason]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Sanction_Edit',
                applyTo: '[name=Sanction]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Paided_Edit',
                applyTo: '[name=Paided]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.PenaltyAmount_Edit',
                applyTo: '[name=PenaltyAmount]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateTransferSsp_Edit',
                applyTo: '[name=DateTransferSsp]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TerminationBasement_Edit',
                applyTo: '[name=TerminationBasement]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TerminationDocumentNum_Edit',
                applyTo: '[name=TerminationDocumentNum]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Executant_Edit',
                applyTo: '[name=Executant]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Contragent_Edit',
                applyTo: '[name=Contragent]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SurName_Edit',
                applyTo: '[name=SurName]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CitizenshipType_Edit',
                applyTo: '[name=CitizenshipType]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Name_Edit',
                applyTo: '[name=Name]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Patronymic_Edit',
                applyTo: '[name=Patronymic]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IdentityDocumentType_Edit',
                applyTo: '[name=IdentityDocumentType]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SerialAndNumberDocument_Edit',
                applyTo: '[name=SerialAndNumberDocument]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BirthDate_Edit',
                applyTo: '[name=BirthDate]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IssueDate_Edit',
                applyTo: '[name=IssueDate]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BirthPlace_Edit',
                applyTo: '[name=BirthPlace]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IssuingAuthority_Edit',
                applyTo: '[name=IssuingAuthority]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Address_Edit',
                applyTo: '[name=Address]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Company_Edit',
                applyTo: '[name=Company]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.MaritalStatus_Edit',
                applyTo: '[name=MaritalStatus]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.RegistrationAddress_Edit',
                applyTo: '[name=RegistrationAddress]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DependentCount_Edit',
                applyTo: '[name=DependentCount]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Salary_Edit',
                applyTo: '[name=Salary]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ResponsibilityPunishment_Edit',
                applyTo: '[name=ResponsibilityPunishment]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateFio_Edit',
                applyTo: '[name=DelegateFio]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProcurationNumber_Edit',
                applyTo: '[name=ProcurationNumber]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProcurationDate_Edit',
                applyTo: '[name=ProcurationDate]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateCompany_Edit',
                applyTo: '[name=DelegateCompany]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateResponsibilityPunishment_Edit',
                applyTo: '[name=DelegateResponsibilityPunishment]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProtocolExplanation_Edit',
                applyTo: '[name=ProtocolExplanation]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.AccusedExplanation_Edit',
                applyTo: '[name=AccusedExplanation]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IsInTribunal_Edit',
                applyTo: '[name=IsInTribunal]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.RejectionSignature_Edit',
                applyTo: '[name=RejectionSignature]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ResidencePetition_Edit',
                applyTo: '[name=ResidencePetition]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Citizenship_Edit',
                applyTo: '[name=Citizenship]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TribunalName_Edit',
                applyTo: '[name=TribunalName]',
                selector: 'tatarstanprotocolgjieditpanel'
            },
            {
                name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.OffenseAddress_Edit',
                applyTo: '[name=OffenseAddress]',
                selector: 'tatarstanprotocolgjieditpanel'
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