﻿Ext.define('B4.view.wizard.export.additionalservices.ExportAdditionalServicesWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.additionalservices.AdditionalServicesParametersStepFrame', { wizard: this })];
    }
});