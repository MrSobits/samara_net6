Ext.define('B4.view.wizard.export.suppliernotificationoforderexecution.SupplierNotificationsOfOrderExecutionParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.form.MonthPicker'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    defaults: {
        margin: 5,
        padding: '10 5 0 10',
        labelWidth: 120,
        allowBlank: false,
        labelAlign: 'right',
        editable: false
    },
    items: [
        {
            xtype: 'b4monthpicker',
            name: 'ReportingPeriod',
            itemId: 'reportingPeriod',
            fieldLabel: 'Отчётный период',
            format: 'm-Y'
        }
    ],

    firstInit: function () {
        var me = this;
    },

    getParams: function () {
        var me = this;
        return {
            reportingPeriod: me.wizard.down('#reportingPeriod').getValue(),
            selectedList : 'ALL'
        };
    }
});