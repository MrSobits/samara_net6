Ext.define('B4.view.wizard.dictionary.udatestates.PackagesStepFrame', {
    extend: 'B4.view.wizard.package.BasePackagesStepFrame',

    doBackward: function () {
        this.wizard.setCurrentStep('start');
    },

    doForward: function () {
        var me = this;

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('UpdateStates', 'Dictionary'),
            params: {
                packageIds: Ext.Array.pluck(me.wizard.packages, 'Id')
            },
            timeout: 9999999
        }).next(function (response) {

            me.wizard.result = {
                state: 'success',
                message: 'Обновление статусов справочников успешно выполнено.'
            }
            me.wizard.setCurrentStep('finish');

            me.wizard.unmask();

        }, me).error(function (e) {
            me.wizard.result = {
                state: 'error',
                message: e.message || 'Не удалось обновить статусы справочников'
            };
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;
    }
});
