Ext.define('B4.view.wizard.export.completedWork.ExportCompletedWorkWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.completedWork.CompletedWorkParametersStepFrame', { wizard: this })];
    }
});