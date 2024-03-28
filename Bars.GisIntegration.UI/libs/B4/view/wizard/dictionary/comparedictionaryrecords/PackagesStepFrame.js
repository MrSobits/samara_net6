Ext.define('B4.view.wizard.dictionary.comparedictionaryrecords.PackagesStepFrame', {
    extend: 'B4.view.wizard.package.BasePackagesStepFrame',

    doBackward: function () {
        this.wizard.setCurrentStep('start');
    },

    doForward: function () {
        var me = this;

        if (me.wizard.gisDictionaries) {
            me.wizard.setCurrentStep('compareRecords');
            return;
        }

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetRecordComparisonResult', 'Dictionary'),
            params: {
                dictionaryCode: me.wizard.dictionaryCode,
                packageIds: Ext.Array.pluck(me.wizard.packages, 'Id')
            },
            timeout: 9999999
        }).next(function (response) {
            me.wizard.comparisonResult = Ext.JSON.decode(response.responseText).data;

            me.wizard.setCurrentStep('compareRecords');

            me.wizard.unmask();

        }, me).error(function (e) {
            me.wizard.result = {
                state: 'error',
                message: e.message || 'Не удалось получить список записей справочника'
            };
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;
    }
});
