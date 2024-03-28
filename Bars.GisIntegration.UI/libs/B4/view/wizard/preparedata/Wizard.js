Ext.define('B4.view.wizard.preparedata.Wizard', {
    extend: 'B4.view.wizard.WizardWindow',
    title: 'Мастер подготовки данных',

    exporter_Id: undefined,
    dataSupplierIsRequired: false,
    dataSupplierIds: undefined,
    autoDataSupplier: false,

    openExtender: false,
    openTaskTree: false,
    extenderClassName: 'B4.view.wizard.preparedata.Wizard',

    initialStepId: 'start',

    getStepsFrames: function () {
        var me = this,
            result = [];

        result.push(Ext.create('B4.view.wizard.preparedata.StartStepFrame', { wizard: me }));
        result.push(Ext.create('B4.view.wizard.preparedata.DataSupplierStepFrame', { wizard: me }));
        result = result.concat(me.getParametersStepFrames());
        result.push(Ext.create('B4.view.wizard.WizardFinishStepFrame', { wizard: me }));

        return result;
    },

    getParametersStepFrames: function () {
        return Ext.create('B4.view.wizard.preparedata.ParametersStepFrame', { wizard: this });
    }
});