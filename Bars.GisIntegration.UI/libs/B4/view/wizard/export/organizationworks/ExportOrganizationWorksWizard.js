Ext.define('B4.view.wizard.export.organizationworks.ExportOrganizationWorksWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.organizationworks.OrganizationWorksParametersStepFrame', { wizard: this })];
    }
});