Ext.define('B4.view.wizard.export.inspectionplan.ExportInspectionPlanWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.inspectionplan.InspectionPlanParametersStepFrame', { wizard: this })];
    }
});