Ext.define('B4.view.wizard.senddata.ValidationResultStepFrame', {
    extend: 'B4.view.wizard.WizardBaseStepFrame',
    wizard: undefined,
    stepId: 'validationResult',
    title: 'Результаты валидации',
    layout: 'fit',
    requires: [
        'B4.view.integrations.gis.ValidationResultGrid'
    ],
    items: [{
        xtype: 'validationresultgrid'
    }],

    init: function () {
        var me = this,
            validateResults = me.wizard.validateResults,
            hasValidateResults = validateResults && validateResults.length !== 0,
            validateResultGrid = me.down('validationresultgrid'),
            validateResultGridStore = validateResultGrid.getStore(),
            recordsCount = validateResultGridStore.getCount();

        if (recordsCount === 0 && hasValidateResults === true) {
            validateResultGridStore.loadData(validateResults);
        }
    },

    doBackward: function () {
        this.wizard.setCurrentStep('start');
    },

    doForward: function () {
        var me = this;
        if (me.wizard.uploadAttachmentsResult && me.wizard.uploadAttachmentsResult.length > 0) {
            me.wizard.setCurrentStep('uploadAttachmentResult');
        }else if (me.wizard.packages && me.wizard.packages.length > 0) {
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