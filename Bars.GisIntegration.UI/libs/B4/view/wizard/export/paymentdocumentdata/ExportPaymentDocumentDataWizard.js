Ext.define('B4.view.wizard.export.paymentdocumentdata.ExportPaymentDocumentDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [
            Ext.create('B4.view.wizard.export.paymentdocumentdata.PaymentDocumentDataParametersStepFrame', { wizard: this })
        ];
    }
});
