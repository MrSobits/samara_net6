Ext.define('B4.view.wizard.export.workingplan.ExportWorkingPlanDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.workingplan.WorkingPlanDataParametersStepFrame', { wizard: this })];
    }
});