Ext.define('B4.view.wizard.export.supplyresourcecontract.ExportSupplyResourceContractWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.supplyresourcecontract.SupplyResourceContractParametersStepFrame', { wizard: this })];
    }
});