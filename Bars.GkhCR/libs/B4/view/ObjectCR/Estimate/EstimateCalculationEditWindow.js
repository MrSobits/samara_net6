Ext.define('B4.view.objectcr.estimate.EstimateCalculationEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    minHeight: 300,
    minWidth: 600,
    maximizable: true,
    maximized: true,
    resizable: true,

    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.view.Control.GkhDecimalField',

        'B4.store.objectcr.TypeWorkCr',
        'B4.view.objectcr.estimate.Grid',
        'B4.view.objectcr.estimate.ResStatGrid',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.enums.YesNo'
    ],
    
    title: 'Сметный расчет по работе',
    alias: 'widget.estimatecalcwin',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            layout: 'anchor',
                            title: 'Основная информация',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            autoScroll: true,
                            frame: true,
                            defaults: {
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    title: 'Общие сведения',
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Тип сметы',
                                            enumName: 'B4.enums.EstimationType',
                                            includeEmpty: false,
                                            name: 'EstimationType',
                                            value: 0,
                                            labelWidth: 150,
                                            labelAlign: 'right',
                                            hidden: true
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'TypeWorkCr',
                                            itemId: 'sfTypeWorkCr',
                                            fieldLabel: 'Вид работы',
                                           

                                            store: 'B4.store.objectcr.TypeWorkCr',
                                            textProperty: 'WorkName',
                                            columns: [
                                                { text: 'Вид работы', dataIndex: 'WorkName', flex: 1 },
                                                { text: 'Разрез финансирования', dataIndex: 'FinanceSourceName', flex: 1 }
                                            ],
                                            flex: 0.5,
                                            editable: false,
                                            allowBlank: false,
                                            labelWidth: 150,
                                            labelAlign: 'right'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'container',
                                                layout: { type: 'vbox', align: 'stretch' },
                                                defaults: {
                                                    xtype: 'gkhdecimalfield',
                                                    labelWidth: 150,
                                                    labelAlign: 'right',
                                                    width: 300
                                                },
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    items: [
                                                        {
                                                            name: 'TotalDirectCost',
                                                            itemId: 'dfTotalDirectCost',
                                                            fieldLabel: 'Итого прямые затраты'
                                                        },
                                                        {
                                                            name: 'OverheadSum',
                                                            itemId: 'dfOverheadSum',
                                                            fieldLabel: 'Накладные расходы'
                                                        },
                                                        {
                                                            name: 'OtherCost',
                                                            itemId: 'dfOtherCost',
                                                            fieldLabel: 'Другие затраты'
                                                        },
                                                        {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            fieldLabel: 'Выводить документ на портал',
                                                            name: 'UsedInExport',
                                                            store: B4.enums.YesNo.getStore(),
                                                            displayField: 'Display',
                                                            valueField: 'Value'
                                                        }
                                                    ]
                                                },
                                                {
                                                    items: [
                                                        {
                                                            name: 'EstimateProfit',
                                                            itemId: 'dfEstimateProfit',
                                                            fieldLabel: 'Сметная прибыль'
                                                        },
                                                        {
                                                            name: 'Nds',
                                                            itemId: 'dfNds',
                                                            fieldLabel: 'НДС'
                                                        },
                                                        {
                                                            name: 'TotalEstimate',
                                                            itemId: 'dfTotalEstimate',
                                                            fieldLabel: 'Итого по смете'
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsEstimateDocument',
                                    title: 'Документ сметы',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'EstimateDocumentName',
                                            fieldLabel: 'Документ',
                                            maxLength: 300,
                                            labelWidth: 150,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'container',
                                                layout: { type: 'vbox', align: 'stretch' },
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    defaults: {
                                                        labelWidth: 150,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'EstimateDocumentNum',
                                                            fieldLabel: 'Номер',
                                                            maxLength: 300
                                                        },
                                                        {
                                                            xtype: 'b4filefield', editable: false,
                                                            name: 'EstimateFile',
                                                            fieldLabel: 'Файл'
                                                        }
                                                    ]
                                                },
                                                {
                                                    defaults: {
                                                        labelWidth: 150,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'EstimateDateFrom',
                                                            fieldLabel: 'от',
                                                            format: 'd.m.Y'
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsResourceStatmentDocument',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    title: 'Документ ведомости ресурсов',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ResourceStatmentDocumentName',
                                            fieldLabel: 'Документ',
                                            maxLength: 300,
                                            labelWidth: 150,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                xtype: 'container',
                                                layout: { type: 'vbox', align: 'stretch' },
                                                defaults: {
                                                    labelWidth: 150,
                                                    labelAlign: 'right'
                                                },
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ResourceStatmentDocumentNum',
                                                            fieldLabel: 'Номер',
                                                            maxLength: 300
                                                        },
                                                        {
                                                            xtype: 'b4filefield', editable: false,
                                                            name: 'ResourceStatmentFile',
                                                            fieldLabel: 'Файл'
                                                        }
                                                    ]
                                                },
                                                {
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'ResourceStatmentDateFrom',
                                                            fieldLabel: 'от',
                                                            format: 'd.m.Y'
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsFileEstimateDocument',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    title: 'Документ файла сметы',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'FileEstimateDocumentName',
                                            fieldLabel: 'Документ',
                                            maxLength: 300,
                                            labelWidth: 150,
                                            labelAlign: 'right'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'container',
                                                layout: { type: 'vbox', align: 'stretch' },

                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    defaults: {
                                                        labelWidth: 150,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'FileEstimateDocumentNum',
                                                            fieldLabel: 'Номер',
                                                            maxLength: 300

                                                        },
                                                        {
                                                            xtype: 'b4filefield', editable: false,
                                                            name: 'FileEstimateFile',
                                                            fieldLabel: 'Файл'
                                                        }
                                                    ]
                                                },
                                                {
                                                    defaults: {
                                                        labelWidth: 150,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            name: 'FileEstimateDateFrom',
                                                            fieldLabel: 'от',
                                                            format: 'd.m.Y'
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
                            xtype: 'estimategrid',
                            margins: -1
                        },
                        {
                            xtype: 'resstatgrid',
                            margins: -1
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
                                { xtype: 'b4savebutton' },
                                { xtype: 'gkhbuttonimport' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                },
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
