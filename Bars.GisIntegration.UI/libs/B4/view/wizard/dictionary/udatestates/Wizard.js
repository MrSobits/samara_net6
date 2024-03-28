Ext.define('B4.view.wizard.dictionary.udatestates.Wizard', {
    extend: 'B4.view.wizard.WizardWindow',

    title: 'Мастер обновления статусов справочников',

    needSign: true,

    //пакеты
    packages: undefined,

    signer: undefined,

    getStepsFrames: function () {
        var me = this,
            result = [];

        result.push(Ext.create('B4.view.wizard.dictionary.udatestates.StartStepFrame', {wizard: me}));
        result.push(Ext.create('B4.view.wizard.dictionary.udatestates.PackagesStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.WizardFinishStepFrame', { wizard: me }));

        return result;
    }
});