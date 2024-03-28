Ext.define('B4.view.wizard.export.notificationoforderexecution.NotificationsOfOrderExecutionWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.notificationoforderexecution.NotificationsOfOrderExecutionParametersStepFrame', { wizard: this })];
    }
});