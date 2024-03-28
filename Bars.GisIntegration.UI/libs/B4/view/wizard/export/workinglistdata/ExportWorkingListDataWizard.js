Ext.define('B4.view.wizard.export.workinglistdata.ExportWorkingListDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.workinglistdata.WorkingListDataParametersStepFrame', { wizard: this })];
    }
});