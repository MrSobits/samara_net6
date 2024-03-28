Ext.define('B4.view.wizard.export.notificationData.ExportNotificationDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.notificationData.NotificationDataParametersStepFrame', { wizard: this })];
    }
});