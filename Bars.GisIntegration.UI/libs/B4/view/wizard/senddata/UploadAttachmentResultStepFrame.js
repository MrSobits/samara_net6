Ext.define('B4.view.wizard.senddata.UploadAttachmentResultStepFrame', {
    extend: 'B4.view.wizard.WizardBaseStepFrame',
    wizard: undefined,
    stepId: 'uploadAttachmentResult',
    title: 'Результаты загрузки вложений',
    layout: 'fit',
    requires: [
        'B4.view.integrations.gis.UploadResultGrid'
    ],
    items: [{
        xtype: 'uploadresultgrid'
    }],

    init: function () {
        var me = this,
            uploadAttachmentsResult = me.wizard.uploadAttachmentsResult,
            hasUploadAttachmentsResult = uploadAttachmentsResult && uploadAttachmentsResult.length !== 0,
            uploadAttachmentsResultGrid = me.down('uploadresultgrid'),
            uploadAttachmentsResultStore = uploadAttachmentsResultGrid.getStore(),
            recordsCount = uploadAttachmentsResultStore.getCount();

        if (recordsCount === 0 && hasUploadAttachmentsResult === true) {
            uploadAttachmentsResultStore.loadData(uploadAttachmentsResult);
        }
    },

    doBackward: function () {
        var me = this,
            validateResults = me.wizard.validateResults;

        me.wizard.setCurrentStep(validateResults && validateResults.length !== 0 ? 'validationResult' : 'start');
    },

    doForward: function () {
        var me = this;
        if (me.wizard.packages && me.wizard.packages.length > 0) {
            me.wizard.setCurrentStep('packagesPreview');
        } else {
            me.wizard.result = {
                success: false,
                message: 'Нет данных для отправки'
            };
            me.wizard.setCurrentStep('finish');
        }
    }
});
