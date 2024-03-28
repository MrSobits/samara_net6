Ext.define('B4.view.wizard.export.capitalRepairPlan.ImportCapitalRepairPlanWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.capitalRepairPlan.CapitalRepairPlanParametersStepFrame', { wizard: this })];
    }
});