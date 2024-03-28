Ext.define('B4.view.objectoutdoorcr.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.objectoutdoorcreditpanel',
    title: 'Паспорт объекта',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.realityobj.RealityObjectOutdoor',
        'B4.ux.button.Save',
        'B4.store.dict.RealityObjectOutdoorProgram'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObjectOutdoor',
                            textProperty: 'Name',
                            fieldLabel: 'Объект',
                            flex: 1,
                            store: 'B4.store.realityobj.RealityObjectOutdoor',
                            columns: [
                                {
                                    text: 'Муниципальное образование',
                                    dataIndex: 'Municipality',
                                    flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListWithoutPaging'
                                    }
                                },
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Код',
                                    dataIndex: 'Code',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Жилые дома двора',
                                    dataIndex: 'RealityObjects',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ],
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObjectOutdoorProgram',
                            textProperty: 'Name',
                            fieldLabel: 'Программа',
                            flex: 1,
                            store: 'B4.store.dict.RealityObjectOutdoorProgram',
                            columns: [
                                { text: 'Программа', dataIndex: 'Name', flex: 1 }
                            ],
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'RealityObjectOutdoorCode',
                            fieldLabel: 'Код двора',
                            readOnly: true
                        },
                        {
                            xtype: 'textarea',
                            name: 'RealityObjects',
                            fieldLabel: 'Жилые дома двора',
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            name: 'WarrantyEndDate',
                            fieldLabel: 'Дата окончания гарантийных обязательств',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'textfield',
                            name: 'GjiNum',
                            fieldLabel: 'Номер ГЖИ',
                            maxLength: 255
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Примечание',
                            maxLength: 255
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Дополнительные данные',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'column'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: {
                                        type: 'anchor'
                                    },
                                    defaults: {
                                        labelWidth: 230,
                                        width: 330,
                                        padding: 5,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'MaxAmount',
                                            fieldLabel: 'Предельная сумма (руб.)',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'FactStartDate',
                                            fieldLabel: 'Фактическая дата начала работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumSmr',
                                            fieldLabel: 'Сумма на СМР',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateGjiReg',
                                            fieldLabel: 'Дата регистрации ГЖИ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEndBuilder',
                                            fieldLabel: 'Дата регистрации организацией осуществляющей Стройконтроль',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateStartWork',
                                            itemId: 'dfDateStartWork',
                                            fieldLabel: 'Дата начала работ согласно заключенному договору',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'FactAmountSpent',
                                            fieldLabel: 'Фактически освоенная сумма (руб.)',
                                            anchor: '100%'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: {
                                        type: 'anchor'
                                    },
                                    defaults: {
                                        labelWidth: 230,
                                        width: 330,
                                        padding: 5,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'FactEndDate',
                                            fieldLabel: 'Фактическая дата окончания работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumSmrApproved',
                                            fieldLabel: 'Утвержденная сумма на СМР',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateStopWorkGji',
                                            fieldLabel: 'Дата остановки работ ГЖИ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateAcceptGji',
                                            fieldLabel: 'Дата принятия ГЖИ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEndWork',
                                            fieldLabel: 'Дата принятия организацией осуществляющей Стройконтроль',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'CommissioningDate',
                                            fieldLabel: 'Дата ввода в эксплуатацию',
                                            format: 'd.m.Y',
                                            anchor: '100%'
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
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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