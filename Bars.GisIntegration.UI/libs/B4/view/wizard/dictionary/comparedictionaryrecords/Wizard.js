Ext.define('B4.view.wizard.dictionary.comparedictionaryrecords.Wizard', {
    extend: 'B4.view.wizard.WizardWindow',

    title: 'Мастер сопоставления записей справочника',

    needSign: true,

    //пакеты
    packages: undefined,

    //результат предварительного сопоставления
    comparisonResult: undefined,

    signer: undefined,

    dictionaryCode: undefined,

    dictionaryName: undefined,

    getStepsFrames: function () {
        var me = this,
            result = [];

        var startStepDescription = me.getStartStepDescription(me.dictionaryName);

        result.push(Ext.create('B4.view.wizard.dictionary.comparedictionaryrecords.StartStepFrame', {
            description: startStepDescription,
            wizard: me
        }));
        result.push(Ext.create('B4.view.wizard.dictionary.comparedictionaryrecords.PackagesStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.dictionary.comparedictionaryrecords.CompareRecordsStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.WizardFinishStepFrame', { wizard: me }));

        return result;
    },

    getStartStepDescription: function (dictionaryName) {

        return 'Вас приветствует мастер сопоставления записей справочника ' + dictionaryName + '.'
            + '<br><br>'
            + 'Для сопоставления необходимо запросить актуальный список записей справочника из ГИС ЖКХ.'
            + '<br><br>'
            + 'Будут сформированы пакеты для получения записей справочника.';
    }
});
