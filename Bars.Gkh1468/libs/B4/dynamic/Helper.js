Ext.define('B4.dynamic.Helper', {
    requires: [
        'B4.form.ComboBox',
        'B4.ux.grid.Panel',
        'B4.model.dynamic.AttributeValue',
        'B4.store.dynamic.AttributeValue'
    ],
    singleton: true,

    //Получить набор редакторов
    getItems: function (attributes, loadOptions) {
        var me = this, items = [];
        Ext.each(attributes, function (meta) {
            switch (meta.Type) {
                //Простой
                case 10:
                    items.push(me.generateSimple(meta));
                    break;
                    //Групповой
                    // 0 - из-за того, что некоторые групповые атрибуты добавились с типом 0
                case 0:
                case 20:
                    items.push(me.generateComplex(meta, loadOptions));
                    break;
                    //Групповой со значением
                case 30:
                    items.push(me.generateComplexWithValue(meta, loadOptions));
                    break;
                    //Групповой-множественный
                case 40:
                    items.push(me.generateMulty(meta, loadOptions));
                    break;
            }
        });
        return items;
    },

    //Простой
    generateSimple: function (meta) {
        return this.generateEditor(meta);
    },

    getLabel: function (meta) {
        var measure = '';
        if (meta.UnitMeasure) {
            measure = meta.UnitMeasure.Name || meta.UnitMeasure;
        }
        return Ext.String.format('{0} ({1})', meta.Name, measure);
    },

    //Групповой
    generateComplex: function (meta, loadOptions) {
        return {
            xtype: 'fieldset',
            title: meta.Name,
            defaults: {
                labelWidth: 340
            },
            items: this.getItems(meta.Childrens, loadOptions)
        };
    },

    //Групповой со значением
    generateComplexWithValue: function (meta, loadOptions) {
        var items = [
            this.generateEditor(meta),
            {
                xtype: 'fieldset',
                title: meta.GroupText,
                defaults: {
                    anchor: '100%',
                    flex: 1,
                    labelWidth: 340
                },
                items: this.getItems(meta.Childrens, loadOptions)
            }
        ];

        items = items.concat(this.getItems());

        return {
            xtype: 'fieldset',
            defaults: {
                labelWidth: 340
            },
            items: items
        };
    },

    //Групповой-множественный
    generateMulty: function (meta, loadOptions) {
        var result = Ext.create('Ext.Panel', {
            localData: {},
            values: [],
            maxValue: 0,
            controllerName: loadOptions.controllerName,
            passpId: loadOptions.passpId,
            partId: loadOptions.partId,
            type: 'multy',
            layout: 'anchor',
            metaId: meta.Id,
            cls: 'expandable-panel-for-multy',
            title: meta.Name,
            collapsible: true,
            defaults: {
                labelWidth: 340
            },
            titleCollapse: true,
            collapsed: true,
            bodyPadding: 5,
            dockedItems: [
                {
                    xtype: 'toolbar',
                    padding: '3 5',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            iconCls: 'icon-add',
                            text: 'Добавить',
                            action: 'add'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-cross',
                            text: 'Удалить',
                            action: 'delete'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-bullet-left',
                            action: 'back',
                            tooltip: 'Переход на предыдущую запись'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-bullet-right',
                            action: 'next',
                            tooltip: 'Переход на следующую запись'
                        },
                        '->',
                        {
                            xtype: 'numberfield',
                            labelAlign: 'right',
                            labelWidth: 100,
                            minValue: 1,
                            dataIndex: 'CurentRow',
                            fieldLabel: 'Текущая запись'
                        },
                        {
                            xtype: 'textfield',
                            labelWidth: 100,
                            labelAlign: 'right',
                            dataIndex: 'Count',
                            value: 0,
                            fieldLabel: 'Всего записей',
                            readOnly: true,
                            editable: false,
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false
                        }
                    ]
                }
            ],
            enableChild: function (enable) {
                var fields, me = this;

                if (me.childEnabled && enable) {
                    return;
                }

                fields = Ext.ComponentQuery.query('[metaId]', me);
                Ext.each(fields, function (f) {
                    if (f.type == 'multy') {
                        f.enableChild(enable);
                    } else {
                        f.setDisabled(!enable);
                    }
                });
            },
            setValue: function (value) {
                var count, valueExist, me = this;
                if (!value) {
                    return;
                }

                this.values = this.values || [];
                value = parseInt(value); //приводим к int

                Ext.each(this.values, function (val) {
                    if (val == value) {
                        valueExist = true;
                        return false;
                    }
                });

                if (!valueExist) {
                    this.values.push(value);

                    if (this.values.length > 0) {
                        me.enableChild(true);
                    }

                    if (this.maxValue < value) {
                        this.maxValue = value;
                    }

                    count = this.values.length;
                    var currentRow = Ext.ComponentQuery.query('[dataIndex="CurentRow"]', this)[0];
                    currentRow.suspendEvents();
                    currentRow.suspendCheckChange++;
                    currentRow.setValue(count);
                    currentRow.suspendCheckChange--;
                    currentRow.resumeEvents();

                    Ext.ComponentQuery.query('[dataIndex="Count"]', this)[0].setValue(count);
                }
            },
            deleteCurrentValue: function () {
                var currentRowField = Ext.ComponentQuery.query('[dataIndex="CurentRow"]', this)[0];
                var countField = Ext.ComponentQuery.query('[dataIndex="Count"]', this)[0];

                var current = currentRowField.getValue();
                this.values.splice(current - 1, 1);
                current -= 1;
                if (this.values.length == 0) {
                    this.reset();
                } else {
                    if (current < 1) {
                        current = 1;
                    }
                    if (this.values.length < current) {
                        current = this.values.length;
                    }

                    currentRowField.setValue(current);
                    countField.setValue(this.values.length);
                    this.loadMultyMetaData();
                }
            },
            getValue: function () {
                var current = this.down('[dataIndex="CurentRow"]').getValue();
                return this.values[current - 1];
            },
            changeValue: function (direction, newValue) {
                var currentField = this.down('[dataIndex="CurentRow"]');
                var current = currentField.getValue();
                var count = this.down('[dataIndex="Count"]').getValue();

                var isChanged = false;
                if (direction == 'next' && current < count) {
                    current += 1;
                    isChanged = true;
                }
                else if (direction == 'back' && current > 1) {
                    current -= 1;
                    isChanged = true;
                } else if (direction == 'no') {
                    if (current < 1) {
                        current = 1;
                        isChanged = true;
                    }
                    else if (count < current) {
                        current = count;
                        isChanged = true;
                    }
                }
                else if (direction == 'set') {
                    if (newValue && current != newValue) {
                        current = newValue;
                        isChanged = true;
                    }
                    if (current < 1 || count == 0) {
                        current = 1;
                        isChanged = true;
                    }
                    else if (count < current) {
                        current = count;
                        isChanged = true;
                    }
                }
                else {
                    return 0;
                }

                if (isChanged) {
                    currentField.setValue(current);
                }
                this.value = this.values[current - 1];
                return this.value;
            },
            reset: function () {
                this.down('[dataIndex="CurentRow"]').setValue();
                this.down('[dataIndex="Count"]').setValue();
                this.removeAll();
                this.isLoaded = false;
                this.collapse();
                this.values = [];
                this.maxValue = 0;
            },
            loadMultyMetaData: function () {
                var me = this;
                var myMask = new Ext.LoadMask(this, { msg: "Загрузка данных..." });
                myMask.show();

                B4.Ajax.request({
                    url: B4.Url.action('GetMetaValues', me.controllerName),
                    method: 'GET',
                    timeout: 300000,
                    params: {
                        providerPassportId: me.passpId,
                        partId: me.partId,
                        metaId: me.metaId,
                        parentValue: me.getValue(),
                        isMulty: true
                    }
                }).next(function (response) {
                    var json = Ext.JSON.decode(response.responseText);
                    me.loadData(json.data);
                    if (me.values.length == 0) {
                        me.enableChild(false);
                    }
                    myMask.hide();
                    myMask.destroy();
                })
                .error(function () {
                    myMask.hide();
                    myMask.destroy();
                    throw new Error('Request error');
                });
            },
            loadData: function (data) {
                var me = this,
                    currentParentValue = me.getValue();

                var panels = Ext.ComponentQuery.query('panel[metaId]:first', me);
                Ext.each(panels, function (f) {
                    f.reset();
                });

                var fields = Ext.ComponentQuery.query('[metaId]', me);

                // выставляем родительское значение. т.к. при установке данных значения могут не проставиться.
                // и сбрасываем групповые множественные, для повторной загрузки.
                Ext.each(fields, function (f) {
                    f.suspendEvents();
                    f.parentValue = currentParentValue;
                    f.valueId = null;
                    if (f.xtype == 'b4combobox') {
                        f.oldValue = null;
                    } else {
                        f.oldValue = '';
                    }
                    f.suspendCheckChange++;
                    if (f.type == 'multy') {
                        f.reset();
                    } else {
                        f.setValue();
                    }
                    f.suspendCheckChange--;
                    f.resumeEvents();
                });

                if (data) {
                    Ext.each(data, function (row) {
                        var editors = Ext.ComponentQuery.query(Ext.String.format('[metaId={0}]', row.MetaId), me);
                        if (editors[0]) {
                            editors[0].suspendEvents();
                            editors[0].suspendCheckChange++;
                            if (editors[0].type == 'multy') {
                                editors[0].valueId = row.ValueId;
                                editors[0].parentValue = row.ParentValue;
                                editors[0].oldValue = row.ValueId;
                                editors[0].setValue(row.ValueId);
                            } else {
                                editors[0].valueId = row.ValueId;
                                editors[0].parentValue = row.ParentValue;
                                editors[0].oldValue = row.Value;
                                editors[0].setValue(row.Value);
                            }
                            editors[0].suspendCheckChange--;
                            editors[0].resumeEvents();
                        }

                    });
                }
            },
            addRecord: function () {
                var me = this,
                    children;

                var valueCountField = Ext.ComponentQuery.query('[dataIndex="Count"]', me)[0];
                var valueCount = (parseInt(valueCountField.getValue()) || 0) + 1;

                var myMask = new Ext.LoadMask(this, { msg: "Загрузка данных..." });
                myMask.show();

                var records = [
                {
                    ProviderPassport: me.passpId,
                    MetaAttribute: me.metaId,
                    Value: me.getValue(),
                    ParentValue: me.parentValue
                }];

                B4.Ajax.request({
                    url: B4.Url.action('Create', me.controllerName),
                    method: 'GET',
                    timeout: 300000,
                    params: { records: Ext.encode(records) }
                }).next(function (response) {
                    var json = Ext.JSON.decode(response.responseText);

                    myMask.hide();
                    myMask.destroy();

                    me.enableChild(true);

                    valueCountField.setValue(valueCount);

                    me.maxValue = me.maxValue + 1;
                    me.valueId = me.value = json.data[0].Id;
                    me.oldValue = '';
                    me.valueIsChange = false;
                    me.values.push(me.valueId);

                    children = Ext.ComponentQuery.query('[metaId]', me);
                    Ext.each(children, function (child) {
                        if (child.type == 'multy') {
                            child.reset();
                        }
                        child.suspendEvents();
                        child.suspendCheckChange++;
                        child.valueId = null;
                        child.oldValue = null;
                        child.parentValue = me.valueId;
                        child.setValue();
                        child.suspendCheckChange--;
                        child.resumeEvents();
                    });

                    Ext.ComponentQuery.query('[dataIndex="CurentRow"]', me)[0].setValue(valueCount);
                })
                .error(function () {
                    myMask.hide();
                    myMask.destroy();
                    throw new Error('Request error');
                });
            }
        });

        return result;
    },

    generateEditor: function (meta) {
        var result = {};
        switch (meta.ValueType) {
            //Строка
            case 10:
                result = {
                    xtype: 'textfield',
                    maxLength: meta.MaxLength > 0 ? meta.MaxLength : 2000,
                    minLength: meta.MinLength || 0,
                    maskRe: meta.Pattern
                };
                break;
                //Целочисленный или дробный
            case 20:
            case 30:
                result = {
                    xtype: 'numberfield',
                    maxLength: meta.MaxLength > 0 ? meta.MaxLength : 50,
                    minLength: meta.MinLength || 0
                };

                if (meta.ValueType == 20) {
                    result.allowDecimals = false;
                } else {
                    result.allowDecimals = true;
                    result.decimalPrecision = meta.Exp || 2;
                    result.decimalSeparator = ',';
                }

                if (!!meta.AllowNegative) {
                    result.minValue = 0;
                }
                break;
                //Справочник
            case 40:
                result = {
                    xtype: 'b4combobox',
                    url: '/MultipurposeGlossaryItem/ListByGlossaryCode?glossaryCode=' + meta.DictCode,
                    fields: ['Value', 'Value'], //потом вернуть Id 
                    displayField: 'Value',
                    valueField: 'Value'//потом вернуть Id
                };
                break;
            default:
                return null;
        }

        result.allowBlank = !meta.Required;
        result.metaId = meta.Id;
        result.fieldLabel = meta.Name;
        result.anchor = '100%';
        result.flex = 1;
        result.GroupKey = meta.GroupKey;

        // Костыль для ExtJS, чтобы нормально возбуждалось событие change при действии пользователя:
        // выделяет _весь_ текст в поле и удаляет через <Backspace>, <Delete> или посредством вырезания
        result.checkChange = function () {
            if (!this.suspendCheckChange) {
                var me = this,
                    newVal = me.getValue(),
                    oldVal = me.oldValue;
                if (!me.isEqual(newVal, oldVal) && !me.isDestroyed) {
                    me.lastValue = newVal;
                    me.fireEvent('change', me, newVal, oldVal);
                    me.onChange(newVal, oldVal);
                }
            }
        };

        return result;
    },

    addMultyItems: function (panel, meta) {
        var atrs = B4.dynamic.Helper.convertMetaData(meta);

        panel.add(B4.dynamic.Helper.getItems(atrs, {
            controllerName: panel.controllerName,
            passpId: panel.passpId,
            partId: panel.partId
        }));
        panel.isLoaded = true;
    },

    convertMetaData: function (meta) {
        var atrs = [];
        Ext.each(meta.Childrens, function (child) {
            child.Attribute.Childrens = B4.dynamic.Helper.convertMetaData(child);
            atrs.push(child.Attribute);
        });

        return atrs;
    }
});