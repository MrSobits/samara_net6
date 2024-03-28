Ext.define('B4.view.wizard.export.devicemetering.ExportMeteringDeviceValuesDataWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',
    requires: [
        'B4.enums.MeteringDeviceValueType',
        'B4.enums.MeteringDeviceType'
    ],

    getParametersStepFrames: function() {
        return [Ext.create('B4.view.wizard.export.devicemetering.ExportMeteringDeviceValuesPreviewStepFrame', { wizard: this })];
    }
});