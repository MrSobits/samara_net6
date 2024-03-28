Ext.define('B4.view.wizard.senddata.XmlPreviewStepFrame', {
    extend: 'B4.view.wizard.package.BasePackagesStepFrame',   

    doBackward: function () {

        var me = this;
        if (me.wizard.uploadAttachmentsResult && me.wizard.uploadAttachmentsResult.length > 0) {
            me.wizard.setCurrentStep('uploadAttachmentResult');
        } else if (me.wizard.validateResults && me.wizard.validateResults.length > 0) {
            me.wizard.setCurrentStep('validationResult');
        } else {
            me.wizard.setCurrentStep('start');
        }
    },

    doForward: function () {
        var me = this;

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('ScheduleSendData', 'GisIntegration'),
            params: {
                taskId: me.wizard.taskId,
                packageIds: Ext.Array.pluck(me.wizard.packages, 'Id')
            },
            timeout: 9999999
        }).next(function (response) {

            me.wizard.result = {
                state: 'success',
                message: 'Задача отправки данных успешно запланирована.'
                    + '<br><br>'
                    + 'Выполнение задачи отражено в реестре задач.'
            }
            me.wizard.setCurrentStep('finish');

            me.wizard.openTaskTree = true;

            me.wizard.unmask();

        }, me).error(function (e) {
            me.wizard.result = {
                state: 'error',
                message: e.message || 'Не удалось запланировать отправку данных'
            };
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;
    }
});