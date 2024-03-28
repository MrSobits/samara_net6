Ext.define('B4.view.wizard.export.orgregistry.ImportOrgRegistryWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.orgregistry.OrgRegistryParametersStepFrame', { wizard: this })];
    }
});