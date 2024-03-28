Ext.define('B4.view.wizard.export.notificationsoforderexecutioncancellation.NotificationsOfOrderExecutionCancellationDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.notificationsoforderexecutioncancellation.NotificationsOfOrderExecutionCancellationDataParametersStepFrame', { wizard: this })];
    }
});