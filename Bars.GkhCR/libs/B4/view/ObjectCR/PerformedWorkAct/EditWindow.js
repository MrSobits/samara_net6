Ext.define('B4.view.objectcr.performedworkact.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    requires: [
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.store.objectcr.TypeWorkCr',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.view.objectcr.performedworkact.RecGrid',
        'B4.view.objectcr.performedworkact.PaymentGrid',
        'B4.view.objectcr.performedworkact.PhotoGrid',
        'B4.view.objectcr.performedworkact.PhotoEditWindow',
        'B4.form.FileField',
        'B4.enums.YesNo'
    ],

    layout: { type: 'vbox', align: 'stretch' },
    minHeight: 300,
    minWidth: 600,
    maximizable: true,
    maximized: true,
    resizable: true,

    closeAction: 'hide',
    trackResetOnLoad: true,

    title: 'Акт выполненных работ',
    alias: 'widget.perfactwin',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    padding: '5 5 10 0',
                    border: false,
                    frame: true,
                    layout: 'column',
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: 'anchor',
                            defaults: {
                                anchor: '100%',
                                labelWidth: 120,
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    readOnly: true,
                                    fieldLabel: 'Объект КР',
                                    labelAlign: 'right',
                                    padding: '5 0 0 0',
                                    width: 500,
                                    itemId: 'tfObjectCr'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'TypeWorkCr',
                                    textProperty: 'WorkName',
                                    itemId: 'sfTypeWorkCr',
                                    fieldLabel: 'Работа',
                                    labelAlign: 'right',
                                    store: 'B4.store.objectcr.TypeWorkCr',
                                    allowBlank: false,
                                    editable: false,
                                    columns: [
                                        { text: 'Вид работы', dataIndex: 'WorkName', flex: 1 },
                                        { text: 'Разрез финансирования', dataIndex: 'FinanceSourceName', flex: 1 }
                                    ],
                                    width: 500
                                },
                                {
                                    xtype: 'b4filefield',
                                    editable: false,
                                    name: 'DocumentFile',
                                    fieldLabel: 'Документ акта',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'b4filefield',
                                    editable: false,
                                    name: 'AdditionFile',
                                    fieldLabel: 'Приложение к акту',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'container',
                                    layout: { type: 'hbox' },
                                    defaults: {
                                        xtype: 'container',
                                        layout: { type: 'vbox', align: 'stretch' },
                                        defaults: {
                                            labelAlign: 'right',
                                            labelWidth: 120,
                                            width: 250
                                        }
                                    },
                                    items: [
                                        {
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNum',
                                                    itemId: 'tfDocumentNum',
                                                    fieldLabel: 'Номер',
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'Volume',
                                                    itemId: 'nfVolume',
                                                    fieldLabel: 'Объем'
                                                },
                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'FactVolume',
                                                    itemId: 'nfFactVolume',
                                                   // tooltip: 'Отсюда объем перейдет в конструктивный элемент на доме при утверждении акта',
                                                    fieldLabel: 'Объем кэ',
                                                    listeners: {
                                                        render: function (c) {
                                                            new Ext.ToolTip({
                                                                target: c.getEl(),
                                                                html: 'Отсюда объем перейдет в конструктивный элемент на доме при утверждении акта'
                                                            });
                                                        }
                                                    } 
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateFrom',
                                                    itemId: 'dfDateFrom',
                                                    fieldLabel: 'от',
                                                    format: 'd.m.Y',
                                                    //tooltip: 'Отсюда год перейдет в конструктивный элемент на доме при утверждении акта',
                                                    allowBlank: false,
                                                    listeners: {
                                                        render: function (c) {
                                                            new Ext.ToolTip({
                                                                target: c.getEl(),
                                                                html: 'Отсюда год перейдет в конструктивный элемент на доме при утверждении акта'
                                                            });
                                                        }
                                                    } 
                                                },
                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'Sum',
                                                    itemId: 'nfSum',
                                                    fieldLabel: 'Сумма',
                                                    allowBlank: false
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateFromTransfer',
                                                    itemId: 'dfDateFromTransfer',
                                                    fieldLabel: 'Дата оплаты',
                                                    format: 'd.m.Y',
                                                    allowBlank: true
                                                },
                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'SumTransfer',
                                                    itemId: 'nfSumTransfer',
                                                    fieldLabel: 'Сумма оплаты',
                                                    allowBlank: true
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'component',
                                                    height: 28
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    labelWidth: 140,
                                                    fieldLabel: 'Факт больше плана',
                                                    name: 'OverLimits',
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: 'anchor',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 200
                            },
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    editable: false,
                                    name: 'CostFile',
                                    fieldLabel: 'Справка о стоимости выполненных работ и затрат'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'UsedInExport',
                                    fieldLabel: 'Выводить документ на портал',
                                    enumName: 'B4.enums.YesNo'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'RepresentativeSigned',
                                    fieldLabel: 'Акт подписан представителем собственников',
                                    enumName: 'B4.enums.YesNo'
                                },
                                {
                                    xtype: 'container',
                                    name: 'RepresentativeNameContainer',
                                    layout: { type: 'hbox', align: 'stretch' },
                                    manualDisabled: false,
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    padding: '0 0 10 0',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'RepresentativeSurname',
                                            fieldLabel: 'Фамилия представителя',
                                            labelWidth: 200,
                                            flex: 2
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'RepresentativeName',
                                            fieldLabel: 'Имя',
                                            labelWidth: 75,
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'RepresentativePatronymic',
                                            fieldLabel: 'Отчество',
                                            labelWidth: 75,
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'ExploitationAccepted',
                                    fieldLabel: 'Принято в эксплуатацию',
                                    enumName: 'B4.enums.YesNo'
                                },
                                {
                                    xtype: 'container',
                                    layout: { type: 'hbox', align: 'stretch' },
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 200,
                                        allowBlank: false,
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'WarrantyStartDate',
                                            fieldLabel: 'Дата начала гарантийного срока',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'WarrantyEndDate',
                                            fieldLabel: 'Дата окончания гарантийного срока',
                                            format: 'd.m.Y',
                                            labelWidth: 230
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'perfworkactrecgrid',
                            margins: -1,
                            flex: 1
                        },
                        {
                            xtype: 'panel',
                            title: 'Оплата акта выполненных работ',
                            itemId: 'tabPayment',
                            margins: -1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            items: [
                                {
                                    xtype: 'perfworkactpaymentgrid',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'perfworkactphotogrid',
                            margins: -1,
                            flex: 1
                        },
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
                                { xtype: 'b4savebutton' }
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
                        },
                        {
                            xtype: 'buttongroup',
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