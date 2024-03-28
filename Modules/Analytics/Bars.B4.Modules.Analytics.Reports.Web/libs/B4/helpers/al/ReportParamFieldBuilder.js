Ext.define('B4.helpers.al.ReportParamFieldBuilder', {
    requires: [
        'B4.enums.al.ParamType',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.view.al.SqlQueryParamSelectField',
        'B4.form.GkhMonthPicker'
    ],
    singleton: true,
    collectionAspect: [],
    
    getAspects: function () {
        return this.collectionAspect;
    },

    getFieldConfig: function (param, dv, id) {
        var me = this,
            paramEnum = B4.enums.al.ParamType;
        switch (param.ParamType) {
            case paramEnum.Bool:
                return me._getConfig('checkbox', param);
            case paramEnum.Number:
                return me._getConfig('numberfield', param);
            case paramEnum.Text:
                return me._getConfig('textfield', param);
            case paramEnum.Enum:
                return me._getEnumField(param, dv);
            case paramEnum.Catalog:
                return me._getCatalogField(param, dv);
            case paramEnum.Date:
                return me._getConfig('datefield', param);
            case paramEnum.SqlQuery:
                return me._getSqlQueryField(param, dv, id);
            case paramEnum.MonthYear:
                return me._getMonthYear(param, dv);
        }
        Ext.Error.raise('Unknown param');
    },

    _getMonthYear: function (param, dv) {
        var field = Ext.create('B4.form.GkhMonthPicker',
            {
                name: param.Name,
                labelAlign: dv.labelAlign,
                labelWidth: dv.labelWidth,
                fieldLabel: param.Label,
                editable: false,
                allowBlank: !param.Required
             });

        if (param.Multiselect) {
            field.selectionMode = 'MULTI';
        }

        return field;
    },

    _getConfig: function (xtype, param) {
        return {
            xtype: xtype,
            name: param.Name,
            fieldLabel: param.Label,
            allowBlank: !param.Required
        };
    },

    _getCatalogField: function(param, dv) {
        var field = Ext.create(param.Catalog.SelectFieldClass, {
            name: param.Name,
            labelAlign: dv.labelAlign,
            labelWidth: dv.labelWidth,
            fieldLabel: param.Label,
            allowBlank: !param.Required
        });

        Ext.each(field.aspects, function (aspectConfig) {
            var index = this.collectionAspect.findIndex(function(x) {
                return x.name == aspectConfig.name;
            });

            if (index ==-1) {
                this.collectionAspect.push(aspectConfig);
            }
        }, this);

        if (param.Multiselect) {
            field.selectionMode = 'MULTI';
        }

        return field;
    },

    _getEnumField: function (param, dv) {
        var field = Ext.widget('b4enumcombo', {
            name: param.Name,
            labelAlign: dv.labelAlign,
            labelWidth: dv.labelWidth,
            fieldLabel: param.Label,
            allowBlank: !param.Required,
            enumName: param.Enum.EnumJsClass
        });

        if (param.Multiselect) {
            field.multiSelect = true;
        }

        return field;
    },

    _getSqlQueryField: function (param, dv, id) {
        var field = Ext.create('B4.view.al.SqlQueryParamSelectField', {
            name: param.Name,
            labelAlign: dv.labelAlign,
            labelWidth: dv.labelWidth,
            fieldLabel: param.Label,
            allowBlank: !param.Required
        });

        if (param.Multiselect) {
            field.selectionMode = 'MULTI';
        }

        var fieldStore = field.getStore();

        fieldStore.on('beforeload', function (store, operation) {
            operation.params = operation.params || {};
            operation.params.Id = id;
        });

        fieldStore.load({
            callback: function (records, operation, success) {
                var msg;
                if (!success) {
                    msg = Ext.JSON.decode(operation.response.responseText).message;
                    Ext.Msg.alert('Ошибка!', !Ext.isString(msg) ? 'Ошибка!' : msg);
                }
            }
        });

        return field;
    }
});