Ext.define('B4.view.wizard.dictionary.comparedictionary.Wizard', {
    extend: 'B4.view.wizard.WizardWindow',

    title: 'Мастер сопоставления справочника',

    needSign: true,

    //пакеты
    packages: undefined,

    //справочники ГИС
    gisDictionaries: undefined,

    signer: undefined,

    dictionaryCode: undefined,

    dictionaryName: undefined,

    getStepsFrames: function () {
        var me = this,
            result = [];

        var startStepDescription = me.getStartStepDescription(me.dictionaryName);

        result.push(Ext.create('B4.view.wizard.dictionary.comparedictionary.StartStepFrame', {
            description: startStepDescription,
            wizard: me
        }));
        result.push(Ext.create('B4.view.wizard.dictionary.comparedictionary.PackagesStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.dictionary.comparedictionary.GisDictionariesStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.WizardFinishStepFrame', { wizard: me }));

        return result;
    },

    getStartStepDescription: function (dictionaryName) {

        return 'Вас приветствует мастер сопоставления справочника ' + dictionaryName + '.'
            + '<br><br>'
            + 'Для сопоставления справочника необходимо запросить актуальный список справочников из ГИС ЖКХ.'
            + '<br><br>'
            + 'Будут сформированы пакеты для получения списка справочников.';
    }
});
