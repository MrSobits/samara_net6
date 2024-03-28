Ext.define('B4.aspects.permission.RequestTransferRf', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.requesttransferrfstateperm',

    permissions: [
        { name: 'GkhRf.RequestTransferRf.Edit', applyTo: 'b4savebutton', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.DocumentNum', applyTo: '#tfDocumentNum', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.DateFrom', applyTo: '#dfDateFrom', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.ManagingOrganization', applyTo: '#sfManagingOrganization', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.Inn', applyTo: '#tfInn', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.Phone', applyTo: '#tfPhone', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.Kpp', applyTo: '#tfKpp', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.SettlementAccount', applyTo: '#tfSettlementAccount', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.ContragentBank', applyTo: '#sfContragentBank', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.CorrAccount', applyTo: '#tfCorrAccount', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.Bik', applyTo: '#tfBik', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.ContractRf', applyTo: '#sfContractRf', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.ProgramCr', applyTo: '#sfProgramCr', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.TypeProgramRequest', applyTo: '#cbxTypeProgramRequest', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.Performer', applyTo: '#tfPerformer', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.Field.File', applyTo: '#ffFile', selector: '#requestTransferRfEditWindow' },
        { name: 'GkhRf.RequestTransferRf.TransferFund.Create', applyTo: '#requestTransferAddButton', selector: '#transferFundsRfGrid' },
        { name: 'GkhRf.RequestTransferRf.TransferFund.Edit', applyTo: '#transferFundsRfSaveButton', selector: '#transferFundsRfGrid' },
        { name: 'GkhRf.RequestTransferRf.TransferFund.Delete', applyTo: '#requestTransferDeleteColumn', selector: '#transferFundsRfGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});