Ext.define('B4.view.wastecollection.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.wastecollectionplaceeditpanel',
    title: 'Сведения о площадке сбора ТБО и ЖБО',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.store.wastecollection.WasteCollectionPlace',
        'B4.view.Control.GkhTriggerField',
        'B4.form.EnumCombo',
        'B4.form.ComboBox',
        'B4.enums.TypeWaste',
        'B4.enums.TypeWasteCollectionPlace',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Основные характеристики',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.RealityObject',
                            name: 'RealityObject',
                            fieldLabel: 'Адрес площадки мусороприемных камер',
                            textProperty: 'Address',
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
                            allowBlank: false,
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            name: 'Customer',
                            fieldLabel: 'Компания-заказчик',
                            textProperty: 'Name',
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ],
                            editable: false,
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'b4enumcombo',
                                allowBlank: false,
                                editable: false,
                                includeEmpty: false,
                                enumItems: [],
                                hideTrigger: false,
                                labelWidth: 250,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                               {
                                   name: 'TypeWaste',
                                   fieldLabel: 'Тип хранимых БО',
                                   enumName: 'B4.enums.TypeWaste'
                               },
                               {
                                   name: 'TypeWasteCollectionPlace',
                                   fieldLabel: 'Тип объекта',
                                   enumName: 'B4.enums.TypeWasteCollectionPlace'
                               }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                minValue: 0,
                                labelWidth: 250,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                               {
                                   name: 'PeopleCount',
                                   fieldLabel: 'Количество населения, чел.'
                               },
                               {
                                   name: 'ContainersCount',
                                   fieldLabel: 'Количество контейнеров, шт.'
                               }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                minValue: 0,
                                labelWidth: 250,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                               {
                                   name: 'WasteAccumulationDaily',
                                   fieldLabel: 'Накопление ТБО в сутки, кг.'
                               },
                               {
                                   name: 'LandfillDistance',
                                   fieldLabel: 'Расстояние от полигона, км.'
                               }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Comment',
                            fieldLabel: 'Примечание'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    name: 'ExportWaste',
                    title: 'Вывоз ТБО и ЖБО',
                    items: [
                        {
                            xtype: 'radiogroup',
                            width: 500,
                            padding: '0 0 5 0',
                            fieldLabel: 'Вывоз мусора осуществляется',
                            labelWidth: 180,
                            columns: 2,
                            items: [
                               {
                                   name: 'InFact',
                                   boxLabel: 'по факту сбора ТБО'
                               },
                               {
                                   name: 'Scheduled',
                                   boxLabel: 'по расписанию'
                               }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            items: [
                               {
                                   xtype: 'fieldset',
                                   name: 'Winter',
                                   title: 'Зима',
                                   items: [
                                       {
                                           xtype: 'checkboxgroup',
                                           name: 'ExportDaysWinter',
                                           defaults: {
                                               labelAlign: 'top',
                                               padding: '0 5 0 5',
                                               labelSeparator: ''
                                           },
                                           columns: 8,
                                           items: [
                                              {
                                                  name: 'All',
                                                  fieldLabel: 'еж.',
                                                  padding: '0 15 0 5'
                                              },
                                              {
                                                  name: 'Monday',
                                                  fieldLabel: 'пн.'
                                              },
                                              {
                                                  name: 'Tuesday',
                                                  fieldLabel: 'вт.'
                                              },
                                              {
                                                  name: 'Wednesday',
                                                  fieldLabel: 'ср.'
                                              },
                                              {
                                                  name: 'Thursday',
                                                  fieldLabel: 'чт.'
                                              },
                                              {
                                                  name: 'Friday',
                                                  fieldLabel: 'пт.'
                                              },
                                              {
                                                  name: 'Saturday',
                                                  fieldLabel: 'сб.'
                                              },
                                              {
                                                  name: 'Sunday',
                                                  fieldLabel: 'вс.'
                                              }
                                           ]
                                       }
                                   ]
                               },
                               {
                                   xtype: 'component',
                                   width: 50
                               },
                               {
                                   xtype: 'fieldset',
                                   name: 'Summer',
                                   title: 'Лето',
                                   items: [
                                       {
                                           xtype: 'checkboxgroup',
                                           name: 'ExportDaysSummer',
                                           columns: 8,
                                           defaults: {
                                               labelAlign: 'top',
                                               padding: '0 5 0 5',
                                               labelSeparator: ''
                                           },
                                           items: [
                                              {
                                                  name: 'All',
                                                  fieldLabel: 'еж.',
                                                  padding: '0 15 0 5'
                                              },
                                              {
                                                  name: 'Monday',
                                                  fieldLabel: 'пн.'
                                              },
                                              {
                                                  name: 'Tuesday',
                                                  fieldLabel: 'вт.'
                                              },
                                              {
                                                  name: 'Wednesday',
                                                  fieldLabel: 'ср.'
                                              },
                                              {
                                                  name: 'Thursday',
                                                  fieldLabel: 'чт.'
                                              },
                                              {
                                                  name: 'Friday',
                                                  fieldLabel: 'пт.'
                                              },
                                              {
                                                  name: 'Saturday',
                                                  fieldLabel: 'сб.'
                                              },
                                              {
                                                  name: 'Sunday',
                                                  fieldLabel: 'вс.'
                                              }
                                           ]
                                       }
                                   ]
                               }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Общие сведения о компании-подрядчике',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            name: 'Contractor',
                            fieldLabel: 'Наименование',
                            textProperty: 'Name',
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ],
                            editable: false,
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'textfield',
                            name: 'JuridicalAddress',
                            fieldLabel: 'Юридический адрес'
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 250,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                               {
                                   name: 'Inn',
                                   fieldLabel: 'ИНН'
                               },
                               {
                                   name: 'Kpp',
                                   fieldLabel: 'КПП'
                               }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Договор',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 250,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                               {
                                   xtype: 'textfield',
                                   name: 'NumberContract',
                                   fieldLabel: 'Номер договора'
                               },
                               {
                                   xtype: 'datefield',
                                   name: 'DateContract',
                                   fieldLabel: 'Дата договора'
                               }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'FileContract',
                            fieldLabel: 'Файл договора'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Информация о полигоне',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'LandfillAddress',
                            fieldLabel: 'Адрес полигона'
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});