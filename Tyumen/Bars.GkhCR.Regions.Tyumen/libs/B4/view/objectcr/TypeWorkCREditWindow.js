Ext.define('B4.view.objectcr.TypeWorkCrEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    minWidth: 400,
    maxWidth: 700,
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
        'B4.ux.button.Save',
        'B4.view.objectcr.TypeWorkCrWorksGrid',
        'B4.view.objectcr.TypeWorkCrPotentialWorksGrid',
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
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Work',
                    itemId: 'sflWork',
                    fieldLabel: 'Вид работы',                  
                    store: 'B4.store.dict.Work',
                    allowBlank: false,
                    disabled: true
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
                            itemId: 'nfYearRepair',
                            hideTrigger: true,
                            allowDecimals: false,
                            minValue: 1800,
                            maxValue: 2100,
                            negativeText: 'Значение не может быть отрицательным',
                            fieldLabel: 'Год ремонта'
                        },
                        {
                            xtype: 'component'
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
                            labelWidth: 130,
                            xtype: 'gkhdecimalfield',
                            name: 'Volume',
                            itemId: 'dfVolume',
                            fieldLabel: 'Объем'
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
                            xtype: 'checkbox',
                            name: 'IsEmergrncy',
                            labelWidth: 130,
                            fieldLabel: 'Ремонт по ЧС',
                            itemId: 'chbxIsEmergrncy'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1,
                        margin: '5 0 10 0'
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Sum',
                            fieldLabel: 'Сумма (руб.)',
                            itemId: 'dcfSum'
                        },
                        {
                            xtype: 'component'
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
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    itemId: 'workgridstoolbar',
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'typeworkcrworksgrid',
                            flex: 1
                        },
                        {
                            xtype: 'typeworkcrpotentialworksgrid',
                            flex: 1
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
                            labelWidth: 150,
                            xtype: 'numberfield',
                            name: 'MaxCost',
                            hideTrigger: true,
                            fieldLabel: 'Предельная стоимость.',
                            editable: false
                        },
                        { xtype: 'tbfill' },
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