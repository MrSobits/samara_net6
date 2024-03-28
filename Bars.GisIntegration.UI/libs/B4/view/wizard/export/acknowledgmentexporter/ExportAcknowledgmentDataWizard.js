Ext.define('B4.view.wizard.export.acknowledgmentexporter.ExportAcknowledgmentDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [
            Ext.create('B4.view.wizard.export.acknowledgmentexporter.AcknowledgmentDataParametersStepFrame', { wizard: this })
        ];
    }
});
