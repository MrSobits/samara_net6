Ext.define('B4.view.wizard.senddata.StartStepFrame', {
    extend: 'B4.view.wizard.WizardStartStepFrame',

    wizard: undefined,

    doForward: function () {
        var me = this;

        if (me.wizard.validateResults || me.wizard.packages) {
            me.goToNextStep(me.wizard);
            return;
        }

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetPreparingDataTriggerResult', 'TaskTree'),
            params: {
                taskId: me.wizard.taskId
            },
            timeout: 9999999
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText),
                validateResults = json.data.ValidateResult,
                packages = json.data.Packages,
                uploadResult = json.data.UploadAttachmentsResult;

            me.wizard.validateResults = validateResults;
            me.wizard.packages = packages;
            me.wizard.uploadAttachmentsResult = uploadResult;

            me.goToNextStep(me.wizard);

            me.wizard.unmask();

        }, me).error(function (e) {
            me.wizard.result = {
                success: false,
                message: e.message || 'Не удалось получить результаты подготовки данных'
            }
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;
    },

    goToNextStep: function (wizard) {
        var hasValidateResults = wizard.validateResults && wizard.validateResults.length !== 0,
            hasPackages = wizard.packages && wizard.packages.length !== 0,
            hasUploadResult = wizard.uploadAttachmentsResult && wizard.uploadAttachmentsResult.length !== 0;

        if (hasValidateResults) {
            wizard.setCurrentStep('validationResult');
        } else if (hasUploadResult) {
            wizard.setCurrentStep('uploadAttachmentResult');
        } else if (hasPackages) {
            wizard.setCurrentStep('packagesPreview');
        } else {
            wizard.result = {
                success: false,
                message: 'Нет данных для отправки'
            };
            wizard.setCurrentStep('finish');
        }
    }
});
