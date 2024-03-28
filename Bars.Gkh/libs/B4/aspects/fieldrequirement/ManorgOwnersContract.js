Ext.define('B4.aspects.fieldrequirement.ManorgOwnersContract', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.manorgownerscontractfieldrequirement',
    
    init: function() {
        this.requirements = [
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.SetPaymentsFoundation_Rqrd',
                applyTo: '[name=SetPaymentsFoundation]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PaymentProtocolFile_Rqrd',
                applyTo: '[name=PaymentProtocolFile]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PaymentProtocolDescription_Rqrd',
                applyTo: '[name=PaymentProtocolDescription]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.RevocationReason_Rqrd',
                applyTo: '[name=RevocationReason]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.DateLicenceRegister_Rqrd',
                applyTo: '[name=DateLicenceRegister]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServiceName_Rqrd',
                applyTo: '[name=WorkService]',
                selector: 'ownersworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServicPaymentAmount_Rqrd',
                applyTo: '#Type',
                selector: 'ownersworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServiceType_Rqrd',
                applyTo: '[name=PaymentAmount]',
                selector: 'ownersworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.ProtocolNumber_Rqrd',
                applyTo: '[name=ProtocolNumber]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.ProtocolDate_Rqrd',
                applyTo: '[name=ProtocolDate]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.ProtocolFileInfo_Rqrd',
                applyTo: '[name=ProtocolFileInfo]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.OwnersSignedContractFile_Rqrd',
                applyTo: '[name=OwnersSignedContractFile]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.PlannedEndDate_Rqrd',
                applyTo: '[name=PlannedEndDate]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.FileInfo_Rqrd',
                applyTo: '[name=FileInfo]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.AdditionService.Addition_Service_Rqrd',
                applyTo: '[name=Service]',
                selector: 'ownersadditionserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.AdditionService.Addition_StartDate_Rqrd',
                applyTo: '[name=StartDate]',
                selector: 'ownersadditionserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.AdditionService.Addition_EndDate_Rqrd',
                applyTo: '[name=EndDate]',
                selector: 'ownersadditionserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.CommunalService.Communal_Service_Rqrd',
                applyTo: '[name=Service]',
                selector: 'ownerscommunalserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.CommunalService.Communal_StartDate_Rqrd',
                applyTo: '[name=StartDate]',
                selector: 'ownerscommunalserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.CommunalService.Communal_EndDate_Rqrd',
                applyTo: '[name=EndDate]',
                selector: 'ownerscommunalserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation.InputMeteringDeviceValuesBeginDate_Rqrd',
                applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation.InputMeteringDeviceValuesEndDate_Rqrd',
                applyTo: '[name=InputMeteringDeviceValuesEndDate]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation.DrawingPaymentDocumentDate_Rqrd',
                applyTo: '[name=DrawingPaymentDocumentDate]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation.PaymentServicePeriodDate_Rqrd',
                applyTo: '[name=PaymentServicePeriodDate]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.PaymentInformation.PaymentAmount_Rqrd',
                applyTo: '[name=PaymentAmount]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.PaymentInformation.StartDatePaymentPeriod_Rqrd',
                applyTo: '[name=StartDatePaymentPeriod]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.PaymentInformation.EndDatePaymentPeriod_Rqrd',
                applyTo: '[name=EndDatePaymentPeriod]',
                selector: '#manorgContractOwnersEditWindow'
            }
        ];

        this.callParent(arguments);
    }
});