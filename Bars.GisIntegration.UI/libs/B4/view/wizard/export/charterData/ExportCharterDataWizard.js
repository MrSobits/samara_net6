Ext.define('B4.view.wizard.export.charterData.ExportCharterDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.charterData.CharterDataParametersStepFrame', { wizard: this })];
    }
});