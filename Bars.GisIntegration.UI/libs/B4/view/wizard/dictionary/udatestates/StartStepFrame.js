Ext.define('B4.view.wizard.dictionary.udatestates.StartStepFrame', {
    extend: 'B4.view.wizard.WizardStartStepFrame',

    wizard: undefined,

    description: 'Вас приветствует мастер обновления статусов справочников.'
        + '<br><br>'
        + 'Для обновления статусов необходимо запросить актуальный список справочников из ГИС ЖКХ.'
        + '<br><br>'
        + 'Будут сформированы пакеты для получения списка справочников.',

    doForward: function () {

        var me = this;

        if (me.wizard.packages) {
            me.goToNextStep(me.wizard);
            return;
        }

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetDictionaryListPackages', 'Dictionary'),
            params: {
            },
            timeout: 9999999
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText),
                packages = json.data;

            me.wizard.packages = packages;

            me.wizard.setCurrentStep('packagesPreview');

            me.wizard.unmask();

        }, me).error(function (e) {
            me.wizard.result = {
                success: false,
                message: e.message || 'Не удалось получить пакеты с запросами списка справочников'
            }
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;

    }
});
