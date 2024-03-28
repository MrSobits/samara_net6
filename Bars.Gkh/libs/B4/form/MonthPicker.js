/* 
 * Взято из старых исходников B4, так как в новых версиях он отсутствует
 */
Ext.define('B4.form.MonthPicker', {
    extend: 'Ext.form.field.Date',
    alias: 'widget.b4monthpicker',
    selectMonth: null,
    defaultFormat: 'F, Y',
    createPicker: function () {
        var me = this,
            format = Ext.String.format;

        me.format = me.dateFormat || me.defaultFormat;
        return Ext.create('Ext.picker.Month', {
            pickerField: me,
            ownerCt: me.ownerCt,
            renderTo: document.body,
            floating: true,
            hidden: true,
            focusOnShow: true,
            minDate: me.minValue,
            maxDate: me.maxValue,
            disabledDatesRE: me.disabledDatesRE,
            disabledDatesText: me.disabledDatesText,
            disabledDays: me.disabledDays,
            disabledDaysText: me.disabledDaysText,
            format: me.format,
            showToday: me.showToday,
            startDay: me.startDay,
            minText: format(me.minText, me.formatDate(me.minValue)),
            maxText: format(me.maxText, me.formatDate(me.maxValue)),
            listeners: {
                select: { scope: me, fn: me.onSelect },
                monthdblclick: { scope: me, fn: me.onOKClick },
                yeardblclick: { scope: me, fn: me.onOKClick },
                OkClick: { scope: me, fn: me.onOKClick },
                CancelClick: { scope: me, fn: me.onCancelClick }
            },
            keyNavConfig: {
                esc: function () {
                    me.collapse();
                }
            }
        });
    },

    constructor: function (params) {
        var me = this;

        params.format = params.format || 'F, Y';

        me.callParent(arguments);
    },

    onCancelClick: function () {
        var me = this;
        me.selectMonth = null;
        me.collapse();
    },

    onOKClick: function (comp, data) {
        var me = this,
            tempData = me.selectMoth ? me.selectMoth : new Date((data[0] + 1) + '/1/' + data[1]);

        if (tempData) {
            me.setValue(tempData);
            me.fireEvent('select', me, tempData);
        }
        me.collapse();
    },

    onSelect: function (m, d) {
        var me = this;
        me.selectMonth = new Date((d[0] + 1) + '/1/' + d[1]);
    }
});