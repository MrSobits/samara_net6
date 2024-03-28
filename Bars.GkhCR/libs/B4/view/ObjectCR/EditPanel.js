Ext.define('B4.view.objectcr.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.objectCrEditPanel',
    title: 'Паспорт объекта',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 230,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObject',
                            itemId: 'sfRealityObject',
                            textProperty: 'Address',
                            fieldLabel: 'Объект недвижимости',
                            flex: 1,
                            store: 'B4.store.RealityObject',
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
                                    text: 'Адрес',
                                    dataIndex: 'Address',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ],
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'ProgramCr',
                            itemId: 'sfProgramCr',
                            textProperty: 'Name',
                            fieldLabel: 'Программа',
                            flex: 1,
                            store: 'B4.store.dict.ProgramCr',
                            columns: [
                                { text: 'Программа КР', dataIndex: 'Name', flex: 1 }
                            ],
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'WarrantyEndDate',
                            itemId: 'dfWarrantyEndDate',
                            fieldLabel: 'Дата окончания гарантийных обязательств',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'textfield',
                            name: 'ProgramNum',
                            itemId: 'tfProgramNum',
                            fieldLabel: 'Номер по программе',
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'GjiNum',
                            itemId: 'tfGjiNum',
                            fieldLabel: 'Номер ГЖИ',
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'FederalNumber',
                            itemId: 'tfFederalNumber',
                            fieldLabel: 'Федеральный номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            itemId: 'taDescription',
                            fieldLabel: 'Примечание',
                            maxLength: 500
                        },
                        {
                            xtype: 'checkbox',
                            name: 'DeadlineMissed',
                            fieldLabel: 'Срыв сроков'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 230,
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
                                            name: 'MaxKpkrAmount',
                                            itemId: 'dcfMaxKpkrAmount',
                                            fieldLabel: 'Предельная сумма из КПКР (руб.)',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'FactStartDate',
                                            itemId: 'dfFactStartDate',
                                            fieldLabel: 'Фактическая дата начала работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumSmr',
                                            itemId: 'dcfSumSMR',
                                            fieldLabel: 'Сумма на СМР',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumDevolopmentPsd',
                                            itemId: 'dcfSumDevolopmentPSD',
                                            fieldLabel: 'Сумма на разработку экспертизы ПСД',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateGjiReg',
                                            itemId: 'dfDateGjiReg',
                                            fieldLabel: 'Дата регистрации ГЖИ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEndBuilder',
                                            itemId: 'dfDateEndBuilder',
                                            fieldLabel: 'Дата завершения работ подрядчиком',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateStartWork',
                                            itemId: 'dfDateStartWork',
                                            fieldLabel: 'Дата начала работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateCancelReg',
                                            itemId: 'dfDateCancelReg',
                                            fieldLabel: 'Дата отклонения от регистрации',
                                            format: 'd.m.Y',
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
                                            xtype: 'gkhdecimalfield',
                                            name: 'FactAmountSpent',
                                            itemId: 'dcfFactAmountSpent',
                                            fieldLabel: 'Фактически освоенная сумма (руб.)',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'FactEndDate',
                                            itemId: 'dfFactEndDate',
                                            fieldLabel: 'Фактическая дата окончания работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumSmrApproved',
                                            itemId: 'dcfSumSMRApproved',
                                            fieldLabel: 'Утвержденная сумма на СМР',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumTehInspection',
                                            itemId: 'dcfSumTehInspection',
                                            fieldLabel: 'Сумма на технадзор',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateStopWorkGji',
                                            itemId: 'dfDateStopWorkGji',
                                            fieldLabel: 'Дата остановки работ ГЖИ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateAcceptCrGji',
                                            itemId: 'dfDateAcceptCrGji',
                                            fieldLabel: 'Дата принятия КР ГЖИ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEndWork',
                                            itemId: 'dfDateEndWork',
                                            fieldLabel: 'Дата окончания работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateAcceptReg',
                                            itemId: 'dfDateAcceptReg',
                                            fieldLabel: 'Дата принятия на регистрацию',
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