Ext.define('B4.view.infoaboutusecommonfacilities.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.infoaboutusecommonfacilitieseditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'border',
    width: 655,
    height: 731,
    autoScroll: true,
    bodyPadding: 5,
    title: 'Редактирование записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.enums.Gender',
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.TypeContractDi',
        'B4.enums.LesseeTypeDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    height: 150,
                    region: 'north',
                    defaults: {
                        labelWidth: 195,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'KindCommomFacilities',
                            fieldLabel: 'Вид общего имущества',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'AppointmentCommonFacilities',
                            fieldLabel: 'Назначение общего имущества'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'AreaOfCommonFacilities',
                            fieldLabel: 'Площадь общего имущества (заполняется в отношении помещений и земельных участков) (кв.м)',
                            decimalSeparator: ',',
                            allowDecimals: true,
                            allowBlank: true,
                            minValue: 0
                        },
                        {
                            xtype: 'fieldset',
                            layout: 'anchor',
                            title: 'Протокол общего собрания собственников помещений',
                            anchor: '100%',
                            bodyPadding: 5,
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                flex:1
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    defaults: {
                                        xtype: 'container',
                                        layout: 'anchor',
                                        flex: 1,
                                        defaults: {
                                            labelAlign: 'right',
                                            labelWidth: 190,
                                            anchor: '100%'
                                        }
                                    },
                                    layout: {
                                        type: 'hbox',
                                        pack: 'start',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Number',
                                                    labelWidth: 50,
                                                    fieldLabel: 'Номер',
                                                    allowBlank: false,
                                                    maxLength: 300
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    labelWidth: 50,
                                                    name: 'From',
                                                    fieldLabel: 'Дата',
                                                    allowBlank: false,
                                                    width: 290
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'b4filefield',
                                                    name: 'ProtocolFile',
                                                    labelWidth: 50,
                                                    fieldLabel: 'Файл'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            layout: 'anchor',
                            title: 'Договор',
                            anchor: '100%',
                            bodyPadding: 5,
                            defaults: {
                                labelWidth: 180,
                                anchor: '100%',
                                labelAlign: 'right',
                                maxLength: 300
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Тип арендатора',
                                    labelAlign: 'right',
                                    name: 'LesseeType',
                                    enumName: 'B4.enums.LesseeTypeDi',
                                    value: 10,
                                    includeEmpty: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Surname',
                                    fieldLabel: 'Фамилия',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Имя',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Patronymic',
                                    fieldLabel: 'Отчество'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Пол',
                                    labelAlign: 'right',
                                    name: 'Gender',
                                    enumName: 'B4.enums.Gender',
                                    value: 0,
                                    includeEmpty: false
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'BirthDate',
                                    fieldLabel: 'Дата рождения'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'BirthPlace',
                                    fieldLabel: 'Место рождения'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Snils',
                                    fieldLabel: 'СНИЛС'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Ogrn',
                                    fieldLabel: 'ОГРН'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Lessee',
                                    fieldLabel: 'Наименование арендатора',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Inn',
                                    fieldLabel: 'ИНН'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Номер договора',
                                    name: 'ContractNumber'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Предмет договора',
                                    name: 'ContractSubject'
                                },
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    layout: {
                                        type: 'hbox',
                                        pack: 'start',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right',
                                        labelWidth: 180,
                                        anchor: '100%',
                                        width: 290
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'ContractDate',
                                            fieldLabel: 'Дата договора'
                                            
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'SigningContractDate',
                                            fieldLabel: 'Дата подписания договора'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'CostByContractInMonth',
                                    fieldLabel: 'Стоимость по договору в месяц (руб.)',
                                    decimalSeparator: ',',
                                    allowDecimals: true,
                                    allowBlank: true,
                                    minValue: 0
                                },
                                {
                                    xtype: 'combobox',
                                    editable: false,
                                    fieldLabel: 'Тип договора',
                                    store: B4.enums.TypeContractDi.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'TypeContract'
                                },
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    defaults: {
                                        xtype: 'container',
                                        layout: 'anchor',
                                        flex: 1,
                                        defaults: {
                                            labelAlign: 'right',
                                            labelWidth: 190,
                                            anchor: '100%'
                                        }
                                    },
                                    layout: {
                                        type: 'hbox',
                                        pack: 'start',
                                        align: 'stretch'
                                    },
                                    items: [
                                                {
                                                    items: [{
                                                        xtype: 'datefield',
                                                        format: 'd.m.Y',
                                                        name: 'DateStart',
                                                        fieldLabel: 'Дата начала',
                                                        width: 290,
                                                        allowBlank: false
                                                    }]
                                                },
                                                {
                                                    items: [{
                                                        xtype: 'datefield',
                                                        format: 'd.m.Y',
                                                        name: 'DateEnd',
                                                        fieldLabel: 'Дата окончания',
                                                        width: 290,
                                                        allowBlank: false
                                                    }]
                                                }
                                    ]
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'CostContract',
                                    fieldLabel: 'Сумма договора',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'ContractFile',
                                    fieldLabel: 'Файл'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Comment',
                                    fieldLabel: 'Комментарий'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            layout: 'anchor',
                            title: 'Период внесения платы по договору',
                            name: 'PeriodOfMakingPayments',
                            bodyPadding: 2,
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 195,
                                        anchor: '100%'
                                    },
                                    layout: {
                                        type: 'hbox',
                                        pack: 'start',
                                        align: 'stretch'
                                    },
                                    padding: 2,
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            fieldLabel: 'День месяца начала периода',
                                            name: 'DayMonthPeriodIn',
                                            flex: 1.7,
                                            minValue: 1,
                                            maxValue: 30
                                        },
                                        {

                                            xtype: 'checkboxfield',
                                            boxLabel: 'Последний день месяца',
                                            name: 'IsLastDayMonthPeriodIn',
                                            flex: 1.1,
                                            margin: '0, 10'
                                        },
                                        {
                                            xtype: 'checkboxfield',
                                            boxLabel: 'Cледующего месяца',
                                            name: 'IsNextMonthPeriodIn',
                                            flex: 1,
                                            margin: '0, 10'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 195,
                                        anchor: '100%'
                                    },
                                    layout: {
                                        type: 'hbox',
                                        pack: 'start',
                                        align: 'stretch'
                                    },
                                    padding: 2,
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            fieldLabel: 'День месяца окончания периода',
                                            name: 'DayMonthPeriodOut',
                                            flex: 1.7,
                                            minValue: 2,
                                            maxValue: 31
                                        },
                                        {
                                            xtype: 'checkboxfield',
                                            boxLabel: 'Последний день месяца',
                                            name: 'IsLastDayMonthPeriodOut',
                                            flex: 1.1,
                                            margin: '0, 10'
                                        },
                                        {
                                            xtype: 'checkboxfield',
                                            boxLabel: 'Cледующего месяца',
                                            name: 'IsNextMonthPeriodOut',
                                            flex: 1,
                                            margin: '0, 10'
                                        }
                                    ]
                                }
                            ]
                        }

                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});