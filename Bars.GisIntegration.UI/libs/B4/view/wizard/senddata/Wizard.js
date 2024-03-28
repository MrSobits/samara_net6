Ext.define('B4.view.wizard.senddata.Wizard', {
    extend: 'B4.view.wizard.WizardWindow',

    title: 'Мастер отправки данных',

    prepareDataDescription: undefined,

    taskId: undefined,

    needSign: true,

    //пакеты из результата подготовки данных
    packages: undefined,

    //сообщения валидации из результата подготовки данных
    validateResults: undefined,

    //результат загрузки вложений
    uploadAttachmentsResult: undefined,

    signer: undefined,

    requires: [
    ],

    getStepsFrames: function () {
        var me = this,
            result = [];

        var startStepDescription = me.getStartStepDescription(me.prepareDataDescription);

        result.push(Ext.create('B4.view.wizard.senddata.StartStepFrame', {
            description: startStepDescription,
            wizard: me
        }));
        result.push(Ext.create('B4.view.wizard.senddata.ValidationResultStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.senddata.UploadAttachmentResultStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.senddata.XmlPreviewStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.WizardFinishStepFrame', { wizard: me }));

        return result;
    },

    getStartStepDescription: function (prepareDataDescription) {

        var date = Ext.Date.parse(prepareDataDescription.StartPrepareTime, 'Y-m-dTH:i:s');

        return  'Вас приветствует мастер отправки данных.'
            + '<br><br>'
            + 'Дата подготовки данных: ' + Ext.Date.format(date, 'd.m.Y H:i:s')
            + '<br><br>'
            + 'Пользователь: ' + prepareDataDescription.UserName
            + '<br><br>'
            + 'Сформировано пакетов: ' + prepareDataDescription.PackagesCount
            + '<br><br>'
            + 'Сообщений валидации: ' + prepareDataDescription.ValidationMessagesCount;
    }
});
