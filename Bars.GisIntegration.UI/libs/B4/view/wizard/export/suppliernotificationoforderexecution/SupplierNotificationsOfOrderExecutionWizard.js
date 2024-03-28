Ext.define('B4.view.wizard.export.suppliernotificationoforderexecution.SupplierNotificationsOfOrderExecutionWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.suppliernotificationoforderexecution.SupplierNotificationsOfOrderExecutionParametersStepFrame', { wizard: this })];
    }
});