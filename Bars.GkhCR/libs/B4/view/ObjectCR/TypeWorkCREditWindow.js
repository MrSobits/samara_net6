Ext.define('B4.view.objectcr.TypeWorkCrEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 400,
    maxWidth: 600,
    bodyPadding: 5,
    title: 'Вид работы',
    closeAction: 'destroy',

    alias: 'widget.typeworkcreditwindow',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.dict.Work',
        'B4.store.dict.FinanceSource',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'FinanceSource',
                    itemId: 'sflFinanceSource',
                    fieldLabel: 'Разрез финансирования',
                    store: 'B4.store.dict.FinanceSource',
                    allowBlank: true
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Work',
                    itemId: 'sflWork',
                    fieldLabel: 'Вид работы',
                    store: 'B4.store.dict.Work',
                    allowBlank: false,
                    disabled: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'YearRepair',
                            hideTrigger: true,
                            allowDecimals: false,
                            minValue: 1800,
                            maxValue: 2100,
                            negativeText: 'Значение не может быть отрицательным',
                            fieldLabel: 'Год ремонта'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Sum',
                            fieldLabel: 'Сумма план',
                            itemId: 'dcSum'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStartWork',
                            fieldLabel: 'Начало выполнения работ'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEndWork',
                            fieldLabel: 'Окончание выполнения работ'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'SumMaterialsRequirement',
                            fieldLabel: 'Потребность материалов (руб.)',
                            itemId: 'dcfSumMaterialsRequirement'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Volume',
                            fieldLabel: 'Объем',
                            itemId: 'dcfVolume'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'HasPsd',
                            fieldLabel: 'Наличие ПСД',
                            itemId: 'chbxHasPSD'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Sum',
                            fieldLabel: 'Сумма (руб.)',
                            itemId: 'dcfSum'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'CostSum',
                            fieldLabel: 'Сумма факт',
                            itemId: 'dcfSum'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    itemId: 'taDescription',
                    maxLength: 2000,
                    flex: 1
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
                                { xtype: 'b4savebutton' },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Перенести в другой период',
                                    textAlign: 'left',
                                    itemId: 'sendToOtherPeriodButton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});