Ext.define('B4.aspects.permission.tatarstanprotocolgji.FieldRequirement', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.tatarstanprotocolgjifieldrequirement',

    init: function () {
        this.requirements = [
            //реквизиты
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DocumentNumber', applyTo: '[name=DocumentNumber]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DocumentDate', applyTo: '[name=DocumentDate]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Municipality', applyTo: '[name=Municipality]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DateOffense', applyTo: '[name=DateOffense]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.ZonalInspection', applyTo: '[name=ZonalInspection]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.TimeOffense', applyTo: '[name=TimeOffense]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.CheckInspectors', applyTo: '[name=CheckInspectors]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DateSupply', applyTo: '[name=DateSupply]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Pattern', applyTo: '[name=Pattern]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.AnnulReason', applyTo: '[name=AnnulReason]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.UpdateReason', applyTo: '[name=UpdateReason]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Sanction', applyTo: '[name=Sanction]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.PenaltyAmount', applyTo: '[name=PenaltyAmount]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DateTransferSsp', applyTo: '[name=DateTransferSsp]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.TerminationDocumentNum', applyTo: '[name=TerminationDocumentNum]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.SurName', applyTo: '[name=SurName]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.CitizenshipType', applyTo: '[name=CitizenshipType]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Name', applyTo: '[name=Name]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Citizenship', applyTo: '[name=Citizenship]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Patronymic', applyTo: '[name=Patronymic]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.IdentityDocumentType', applyTo: '[name=IdentityDocumentType]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.SerialAndNumberDocument', applyTo: '[name=SerialAndNumberDocument]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.BirthDate', applyTo: '[name=BirthDate]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.IssueDate', applyTo: '[name=IssueDate]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.BirthPlace', applyTo: '[name=BirthPlace]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.IssuingAuthority', applyTo: '[name=IssuingAuthority]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Address', applyTo: '[name=Address]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Company', applyTo: '[name=Company]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.MaritalStatus', applyTo: '[name=MaritalStatus]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.RegistrationAddress', applyTo: '[name=RegistrationAddress]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DependentCount', applyTo: '[name=DependentCount]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Salary', applyTo: '[name=Salary]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DelegateFio', applyTo: '[name=DelegateFio]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.ProcurationNumber', applyTo: '[name=ProcurationNumber]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.ProcurationDate', applyTo: '[name=ProcurationDate]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.DelegateCompany', applyTo: '[name=DelegateCompany]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.ProtocolExplanation', applyTo: '[name=ProtocolExplanation]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.AccusedExplanation', applyTo: '[name=AccusedExplanation]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.TribunalName', applyTo: '[name=TribunalName]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.OffenseAddress', applyTo: '[name=OffenseAddress]', selector: 'tatarstanprotocolgjieditpanel' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Requisites.Citizenship', applyTo: '[name=Citizenship]', selector: 'tatarstanprotocolgjieditpanel' },

            //приложения
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes.Name', applyTo: '[name=Name]', selector: 'tatarstanprotocolgjiannexeditwindow' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes.DocumentDate', applyTo: '[name=DocumentDate]', selector: 'tatarstanprotocolgjiannexeditwindow' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes.File', applyTo: '[name=File]', selector: 'tatarstanprotocolgjiannexeditwindow' },
            { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Annexes.Description', applyTo: '[name=Description]', selector: 'tatarstanprotocolgjiannexeditwindow' },
        ];

        this.callParent(arguments);
    }
});