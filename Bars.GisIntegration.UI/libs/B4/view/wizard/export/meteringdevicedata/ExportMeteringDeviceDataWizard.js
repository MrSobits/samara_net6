Ext.define('B4.view.wizard.export.meteringdevicedata.ExportMeteringDeviceDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.meteringdevicedata.MeteringDeviceDataParametersStepFrame', { wizard: this })];
    }
});