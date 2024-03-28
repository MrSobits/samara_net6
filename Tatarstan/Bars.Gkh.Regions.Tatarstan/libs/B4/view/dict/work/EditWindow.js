Ext.define('B4.view.dict.work.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'workEditWindow',
    title: 'Форма редактирования работы',
    closeAction: 'hide',
    trackResetOnLoad: true,
    width: 500,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.unitmeasure.Grid',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.TypeFinSource',
        'B4.ux.grid.Panel',
        'Ext.ux.CheckColumn',
        'B4.store.dict.WorkTypeFinSource',
        'B4.view.dict.work.FinSourceChecker'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    padding: 0,
                    frame: true,
                    items: [
                        {
                            xtype: 'container',
                            title: 'Основные параметры',
                            flex: 1,
                            padding: 5,
                            layout: { type: 'vbox', align: 'stretch' },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 100
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Наименование',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'UnitMeasure',
                                    fieldLabel: 'Ед. измерения',
                                    store: 'B4.store.dict.UnitMeasure',
                                    editable: false,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Code',
                                    fieldLabel: 'Код',
                                    allowBlank: false,
                                    maxLength: 10
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'ReformCode',
                                    fieldLabel: 'Код реформы',
                                    minValue: 0,
                                    maxLength: 10,
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    negativeText: 'Значение не может быть отрицательным'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'GisCode',
                                    fieldLabel: 'Код ГИС ЖКХ'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'Normative',
                                    fieldLabel: 'Норматив',
                                    minValue: 0
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        flex: 1,
                                        defaults: {
                                            labelAlign: 'right',
                                            labelWidth: 150,
                                            flex: 1
                                        }
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'vbox',
                                            items: [
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'Consistent185Fz',
                                                    fieldLabel: 'Соответствие 185 ФЗ'
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'IsActual',
                                                    fieldLabel: 'Актуальна'
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'WithinShortProgram',
                                                    fieldLabel: 'Проводится в рамках краткосрочной программы'
                                                },
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'vbox',
                                            items: [
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'IsAdditionalWork',
                                                    fieldLabel: 'Доп. работа'
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'IsConstructionWork',
                                                    fieldLabel: 'Работа (услуга) по строительству'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'TypeWork',
                                    editable: false,
                                    fieldLabel: 'Тип работ',
                                    store: B4.enums.TypeWork.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    maxLength: 500,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            title: 'Источники финансирования',
                            items: [
                                {
                                    xtype: 'finsourcechecker',
                                    editWindow: me
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
                            columns: 2,
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
                            columns: 2,
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