Ext.define('B4.view.constructionobject.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.constructionobjeditpanel',
    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    title: 'Паспорт объекта',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.view.Control.GkhDecimalField',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.ux.button.Save',
        'B4.store.dict.RoofingMaterial',
        'B4.store.dict.WallMaterial',
        'B4.store.dict.ResettlementProgram',
        'B4.enums.TypeRoof'
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
                            xtype: 'b4fiasselectaddress',
                            flex: 1,
                            name: 'FiasAddress',
                            flatIsReadOnly: true,
                            fieldsToHideNames: ['tfFlat'],
                            fieldsRegex: {
                                tfHousing: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                },
                                tfBuilding: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                }
                            },
                            fieldLabel: 'Строительный адрес',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.ResettlementProgram',
                            name: 'ResettlementProgram',
                            fieldLabel: 'Программа переселения',
                            editable: false,
                            anchor: '100%',
                            allowBlank: false
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Примечание',
                            maxLength: 500
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
                                            name: 'SumSmr',
                                            fieldLabel: 'Сумма на СМР',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumDevolopmentPsd',
                                            fieldLabel: 'Сумма на разработку экспертизы ПСД',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEndBuilder',
                                            fieldLabel: 'Дата завершения работ подрядчиком',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateStartWork',
                                            fieldLabel: 'Дата начала работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'LimitOnHouse',
                                            fieldLabel: 'Лимит на дом',
                                            anchor: '100%',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberApartments',
                                            fieldLabel: 'Количество квартир',
                                            minValue: 0,
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            allowDecimals: false,
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'ResettleProgNumberApartments',
                                            fieldLabel: 'в т.ч. по программе переселения',
                                            minValue: 0,
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            allowDecimals: false,
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberEntrances',
                                            fieldLabel: 'Количество подъездов',
                                            minValue: 0,
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            allowDecimals: false,
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.RoofingMaterial',
                                            name: 'RoofingMaterial',
                                            fieldLabel: 'Материал кровли',
                                            editable: false,
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.WallMaterial',
                                            name: 'WallMaterial',
                                            fieldLabel: 'Материал стен',
                                            editable: false,
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
                                            name: 'DateStopWork',
                                            fieldLabel: 'Дата остановки работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateResumeWork',
                                            fieldLabel: 'Дата возобновления работ',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateCommissioning',
                                            fieldLabel: 'Дата сдачи в эксплуатацию',
                                            format: 'd.m.Y',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'ReasonStopWork',
                                            fieldLabel: 'Причина остановки работ',
                                            maxLength: 1000,
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'TotalArea',
                                            fieldLabel: 'Общая площадь дома',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberFloors',
                                            fieldLabel: 'Количество этажей',
                                            minValue: 0,
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            allowDecimals: false,
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberLifts',
                                            fieldLabel: 'Количество лифтов',
                                            minValue: 0,
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            allowDecimals: false,
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'TypeRoof',
                                            fieldLabel: 'Тип кровли',
                                            enumName: 'B4.enums.TypeRoof',
                                            anchor: '100%'
                                        },
                                        {
                                            xtype: 'container',
                                            height: 50
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