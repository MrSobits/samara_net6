Ext.define('B4.view.wizard.export.rkiData.ExportRkiDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.rkiData.RkiDataParametersStepFrame', { wizard: this })];
    }
});