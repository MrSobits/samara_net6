Ext.define('B4.view.passport.AttributeEditor', {
    extend: 'Ext.form.Panel',
    
    requires: [
        'B4.form.SelectField',
        'B4.enums.ValueType',
        'B4.ux.button.Save',
        'B4.store.dict.UnitMeasure',
        'B4.model.dict.UnitMeasure',
        'B4.view.dict.UnitMeasure.Grid',
        'B4.store.dict.multipurpose.MultipurposeGlossary',
        'B4.model.DataFiller',
        'B4.store.DataFillerAll',
        'B4.store.DataFiller'
    ],
    autoScroll: true,
    alias: 'widget.attreditor',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.DataFillerAll');
            store.load();

        Ext.apply(me, {
            entity: 'MetaAttribute',

            defaults: {
                xtype: 'textfield',
                margin: 10
            },
            tbar: {
                items: [
                    {
                        xtype: 'b4savebutton'
                    }
                ]
            },
            items: [
                {
                    fieldLabel: 'Наименование',
                    name: 'Name'
                },
                {
                    fieldLabel: 'Тип',
                    name: 'Type',
                    xtype: 'combo',
                    itemId: 'attributeType',
                    editable: false,
                    store: [[10, 'Простой'], [20, 'Групповой'], [30, 'Групповой со значением'], [40, 'Групповой-множественный']]
                },
                {
                    fieldLabel: 'Учитывается в % заполнения',
                    name: 'UseInPercentCalculation',
                    xtype: 'checkbox',
                    itemId: 'useInPercentCalculation',
                    visible: false
                },
                { xtype: 'hiddenfield', name: 'Id' },
                { xtype: 'hiddenfield', name: 'OrderNum' },
                {
                    fieldLabel: 'Код',
                    name: 'Code'
                },
                {
                    xtype: 'combobox',
                    name: 'Parent',
                    displayField: 'Name',
                    editable: false,
                    valueField: 'Id',
                    visible: 'f'
                },
                {
                    xtype: 'combobox',
                    name: 'ParentPart',
                    displayField: 'Name',
                    editable: false,
                    valueField: 'Id',
                    visible: 'f'
                },
                {
                    fieldLabel: 'Тип значения',
                    name: 'ValueType',
                    xtype: 'combobox',
                    store: B4.enums.ValueType.getStore(),
                    displayField: 'Display',
                    itemId: 'attributeValueType',
                    valueField: 'Value',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Системное значение',
                    editable: false,
                    name: 'DataFillerCode',
                    store: 'B4.store.DataFiller',
                    model: 'B4.model.DataFiller',
                    idProperty: 'Code',
                    textProperty: 'Name',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                    updateDisplayedText: function (data) {
                        var me = this, text;
                        if (Ext.isString(data)) {
                            var record = store.findRecord('Code', data);

                            if (record) {
                                text = record.get('Name');
                            }
                        }
                        else {
                            text = data && data[me.textProperty] ? data[me.textProperty] : '';
                            if (Ext.isEmpty(text) && Ext.isArray(data)) {
                                text = Ext.Array.map(data, function (record) { return record[me.textProperty]; }).join();
                            }
                        }
                        me.setRawValue.call(me, text);
                    }
                },
                {
                    fieldLabel: 'Справочник',
                    editable: false,
                    xtype: 'combobox',
                    name: 'DictCode',
                    disabled: true,
                    store: Ext.create('B4.store.dict.multipurpose.MultipurposeGlossary'),
                    displayField: 'Name',
                    valueField: 'Code',
                    allowBlank: false
                },
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Проверка значения дочерних',
                    name: 'ValidateChilds'
                },
                {
                    fieldLabel: 'Текст для группы',
                    name: 'GroupText'
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    fieldLabel: 'Единица измерения',
                    name: 'UnitMeasure',
                    store: 'B4.store.dict.UnitMeasure',
                    model: 'B4.model.dict.UnitMeasure',
                   

                    textProperty: 'Name',
                    allowBlank: false
                },
                {
                    fieldLabel: 'Код интеграции',
                    name: 'IntegrationCode'
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Максимальная длина',
                    name: 'MaxLength',
                    allowDecimals: false,
                    minValue: 0,
                    listeners: {
                        change: function (field, newVal) {
                            var min = field.up('form').down('numberfield[name="MinLength"]');
                            min.setMaxValue(newVal);
                        }
                    }
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Минимальная длина',
                    name: 'MinLength',
                    allowDecimals: false,
                    minValue: 0,
                    listeners: {
                        change: function (field, newVal) {
                            var max = field.up('form').down('numberfield[name="MaxLength"]');
                            max.setMinValue(newVal);
                        }
                    }
                },
                { fieldLabel: 'Маска ввода', name: 'Pattern' },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Знаков после ,',
                    allowDecimals: false,
                    minValue: 0,
                    name: 'Exp'
                },
                { xtype: 'checkbox', fieldLabel: 'Обязательное', name: 'Required' },
                { xtype: 'checkbox', fieldLabel: 'Отрицательное', name: 'AllowNegative' }
            ]
        });

        me.callParent(arguments);
    }
});