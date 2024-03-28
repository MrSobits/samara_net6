Ext.define('B4.view.wizard.export.accountData.ExportAccountDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [
            Ext.create('B4.view.wizard.export.accountData.AccountDataParametersStepFrame', { wizard: this })
        ];
    }
});