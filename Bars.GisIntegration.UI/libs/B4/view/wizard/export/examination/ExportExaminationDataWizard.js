Ext.define('B4.view.wizard.export.examination.ExportExaminationDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [
            Ext.create('B4.view.wizard.export.examination.ExaminationDataParametersStepFrame', { wizard: this })
        ];
    }
});