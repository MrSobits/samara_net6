Ext.define('B4.view.wizard.dictionary.comparedictionary.PackagesStepFrame', {
    extend: 'B4.view.wizard.package.BasePackagesStepFrame',

    doBackward: function () {
        this.wizard.setCurrentStep('start');
    },

    doForward: function () {
        var me = this;

        if (me.wizard.gisDictionaries) {
            me.wizard.setCurrentStep('gisDictionaries');
            return;
        }

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetGisDictionariesList', 'Dictionary'),
            params: {
                packageIds: Ext.Array.pluck(me.wizard.packages, 'Id')
            },
            timeout: 9999999
        }).next(function (response) {
            var gisDictionaries = Ext.JSON.decode(response.responseText);

            me.wizard.gisDictionaries = gisDictionaries;

            me.wizard.setCurrentStep('gisDictionaries');

            me.wizard.unmask();

        }, me).error(function (e) {
            me.wizard.result = {
                state: 'error',
                message: e.message || 'Не удалось получить список справочников ГИС ЖКХ'
            };
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;
    }
});
