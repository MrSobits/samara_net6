Ext.define('B4.view.dict.tariff.TariffInfoFieldSet', {
    extend: 'Ext.form.FieldSet',
    alias: 'widget.tariffinfofieldset',

    title: 'Информация о тарифе',

    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.enums.GisTariffKind',
        'B4.enums.ConsumerType',
        'B4.enums.ConsumerByElectricEnergyType',
        'B4.enums.SettelmentType',
        'B4.view.Control.GkhDecimalField'
    ],
    layout: 'fit',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                padding: '0 0 5 0',
                flex: 1
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'StartDate',
                            fieldLabel: 'Дата начала периода',
                            allowBlank: false,
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'EndDate',
                            fieldLabel: 'Дата окончания периода',
                            allowBlank: false,
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'combobox',
                    name: 'TariffKind',
                    fieldLabel: 'Вид тарифа',
                    store: B4.enums.GisTariffKind.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'container',
                    layout: 'fit',
                    name: 'ElectricTariffBlock',
                    isAllowed: true,
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'ZoneCount',
                            fieldLabel: 'Количество зон',
                            items: [[1, '1'], [2, '2'], [3, '3']],
                            allowBlank: false,
                            editable: false,
                            filter: {
                                xtype: 'numberfield'
                            }
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'TariffValue1',
                                    fieldLabel: 'Значение тарифа 1',
                                    allowBlank: false,
                                    filter: {
                                        xtype: 'numberfield'
                                    },
                                    width: 250
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'TariffValue2',
                                    fieldLabel: 'Значение тарифа 2',
                                    disabled: true,
                                    filter: {
                                        xtype: 'numberfield'
                                    },
                                    width: 250
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'TariffValue3',
                                    fieldLabel: 'Значение тарифа 3',
                                    disabled: true,
                                    filter: {
                                        xtype: 'numberfield'
                                    },
                                    width: 250
                                }
                            ]
                        },
                        
                    ]
                },
                {
                    xtype: 'container',
                    name: 'OtherTariffBlock',
                    isAllowed: true,
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'TariffValue',
                            fieldLabel: 'Тариф',
                            allowBlank: false,
                            width: 270
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 10 0',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'IsNdsInclude',
                            fieldLabel: 'Включая НДС',
                            labelWidth: 150
                        },
                        {
                            xtype: 'checkbox',
                            name: 'IsSocialNorm',
                            fieldLabel: 'В пределах социальной нормы',
                            labelWidth: 192
                        },
                        {
                            xtype: 'checkbox',
                            name: 'IsMeterExists',
                            fieldLabel: 'Наличие прибора учета',
                            labelWidth: 152
                        },
                        {
                            xtype: 'checkbox',
                            name: 'IsElectricStoveExists',
                            fieldLabel: 'Наличие электрической плиты',
                            labelWidth: 192
                        }
                    ]
                },
                {
                    xtype: 'numberfield',
                    name: 'Floor',
                    fieldLabel: 'Этаж'
                },
                {
                    xtype: 'combobox',
                    name: 'ConsumerType',
                    fieldLabel: 'Тип потребителя',
                    store: B4.enums.ConsumerType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'combobox',
                    name: 'SettelmentType',
                    fieldLabel: 'Вид населенного пункта',
                    store: B4.enums.SettelmentType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'combobox',
                    name: 'ConsumerByElectricEnergyType',
                    fieldLabel: 'Тип потребителя по электроэнергии',
                    store: B4.enums.ConsumerByElectricEnergyType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    name: 'RegulatedPeriodAttribute',
                    fieldLabel: 'Дополнительный признак организации в регулируемом периоде'
                },
                {
                    xtype: 'textarea',
                    name: 'BasePeriodAttribute',
                    fieldLabel: 'Дополнительный признак организации в базовом периоде'
                }
            ]
        });

        me.callParent(arguments);
    },

    setVisible: function(isVisible) {
        var me = this;

        if (isVisible) {
            me.show();
        } else {
            me.hide();
        }

        me.setDisabled(!isVisible);
    }
});