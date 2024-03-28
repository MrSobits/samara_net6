Ext.define('B4.view.wizard.export.publicPropertyContractData.ExportPublicPropertyContractDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.publicPropertyContractData.PublicPropertyContractDataParametersStepFrame', { wizard: this })];
    }
});