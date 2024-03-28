Ext.define('B4.aspects.fieldrequirement.ManorgTsjJskContract', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.manorgtsjjskcontractfieldrequirement',
    
    init: function() {
        this.requirements = [
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.StartDatePaymentPeriod_Rqrd',
                applyTo: '[name=StartDatePaymentPeriod]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.EndDatePaymentPeriod_Rqrd',
                applyTo: '[name=EndDatePaymentPeriod]',
                selector: '#jskTsjContractEditWindow'
            },
             {
                 name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyReqiredPaymentAmount_Rqrd',
                 applyTo: '[name=CompanyReqiredPaymentAmount]',
                 selector: '#jskTsjContractEditWindow'
             },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ReqiredPaymentAmount_Rqrd',
                applyTo: '[name=ReqiredPaymentAmount]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyPaymentProtocolFile_Rqrd',
                applyTo: '[name=CompanyPaymentProtocolFile]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyPaymentProtocolDescription_Rqrd',
                applyTo: '[name=CompanyPaymentProtocolDescription]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentAmount_Rqrd',
                applyTo: '[name=PaymentAmount]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentProtocolFile_Rqrd',
                applyTo: '[name=PaymentProtocolFile]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentProtocolDescription_Rqrd',
                applyTo: '[name=PaymentProtocolDescription]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServiceName_Rqrd',
                applyTo: '[name=WorkService]',
                selector: 'jsktsjworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServicPaymentAmount_Rqrd',
                applyTo: '#Type',
                selector: 'jsktsjworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServiceType_Rqrd',
                applyTo: '[name=PaymentAmount]',
                selector: 'jsktsjworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ProtocolNumber_Rqrd',
                applyTo: '[name=ProtocolNumber]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ProtocolFileInfo_Rqrd',
                applyTo: '[name=ProtocolFileInfo]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PlannedEndDate_Rqrd',
                applyTo: '[name=PlannedEndDate]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.FileInfo_Rqrd',
                applyTo: '[name=FileInfo]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.InputMeteringDeviceValuesBeginDate_Rqrd',
                applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.InputMeteringDeviceValuesEndDate_Rqrd',
                applyTo: '[name=InputMeteringDeviceValuesEndDate]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.DrawingPaymentDocumentDate_Rqrd',
                applyTo: '[name=DrawingPaymentDocumentDate]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentServicePeriodDate_Rqrd',
                applyTo: '[name=PaymentServicePeriodDate]',
                selector: '#jskTsjContractEditWindow'
            }
        ];

        this.callParent(arguments);
    }
});