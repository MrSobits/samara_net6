Ext.define('B4.form.GisMonthPicker', {
    extend: 'B4.form.MonthPicker',
    alias: 'widget.b4gismonthpicker',

    constructor: function (params) {
        var me = this;

        params.format = params.format || 'F, Y';
        params.labelWidth = params.labelWidth || 100;
        params.labelAlign = params.labelAlign || 'right';
        params.fieldLabel = params.fieldLabel || 'Месяц';
        params.editable = params.editable || false;

        me.callParent(arguments);
    },
    
    getValue: function () {
        return this.value;
    },

    beforeBlur: function() {
        var me = this,
            focusTask = me.focusTask;

        if (focusTask) {
            focusTask.cancel();
        }
    }
});