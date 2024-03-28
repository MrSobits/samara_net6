Ext.define('B4.model.administration.notify.Message', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NotifyMessage'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectEditDate' },
        { name: 'IsActual' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'Title' },
        {
            name: 'Text',
            convert: function (v) {
                return decodeURI(v);
            },
            serialize: function (v) {
                return encodeURI(v);
            }
        },
        { name: 'ButtonSet' },
        { name: 'User' },

    ],

    set: function (fieldName, newValue) {
        var me = this,
            fields = me.fields,
            single = (typeof fieldName == 'string'),
            field, name,
            values;

        if (single) {
            values = me._singleProp;
            values[fieldName] = newValue;
        } else {
            values = fieldName;
        }

        for (name in values) {
            if (values.hasOwnProperty(name)) {
                if (fields && (field = fields.get(name)) && field.serialize) {
                    field._convert = field.convert;
                    field.convert = field.serialize;
                }
            }
        }

        var res = this.callParent([fieldName, newValue]);

        for (name in values) {
            if (values.hasOwnProperty(name)) {
                if (fields && (field = fields.get(name)) && field.serialize) {
                    field.convert = field._convert;
                }
            }
        }
        return res;
    }
});