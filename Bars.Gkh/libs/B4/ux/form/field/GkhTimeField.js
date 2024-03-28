Ext.define('B4.ux.form.field.GkhTimeField', {
    extend: 'Ext.form.field.Time',

    alias: 'widget.gkhtimefield',

    editable: false,
    minValue: '00:00',
    maxValue: '23:00',
    increment: 60,
    format: 'H:i',
    //altFormats: 'Y-m-d\\TH:i:s',
    lastValue: new Date(),

    setValue: function (value) {
        var locaValue = value;
        if (Ext.isString(value)) {
            locaValue = Ext.Date.parse(value, 'H:i:s');
            if (!Ext.isDefined(locaValue)) {
                locaValue = new Date(value);
            }
        }

        this.callParent([locaValue]);
    },

    getValue: function () {
        if (this.value) {
            return Ext.Date.format(this.value, this.format);
        }

        return null;
    },

    getSubmitValue: function () {
        return this.getValue();
    },

    isEqual: function (date1, date2) {
        if (date1 && date1.getTime && date2 && date2.getTime) {
            return (date1.getTime() === date2.getTime());
        }

        return date1 == date2;
    }
});