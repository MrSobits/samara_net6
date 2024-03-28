Ext.define('B4.view.wizard.export.contractData.ExportContractDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.contractData.ContractDataParametersStepFrame', { wizard: this })];
    }
});