Ext.define('B4.view.suspenseaccount.RoPerfomedWorkActsDistributionGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.roperfomedworkactsdistributiongrid',
    requires: [
        'B4.form.ComboBox',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.form.EnumCombo',
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Close',
        'B4.enums.ActPaymentType'
    ],

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            store: Ext.create('Ext.data.Store', {
                fields: [
                    { name: 'Id' },
                    { name: 'ActId' },
                    { name: 'State' },
                    { name: 'Address' },
                    { name: 'TypeWorkCr' },
                    { name: 'ActSum' },
                    { name: 'PaymentType' },
                    { name: 'PaymentSum' },
                    { name: 'DatePayment' }
                ]
            }),
            columns: [
                {
                    dataIndex: 'State',
                    text: 'Статус акта',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'TypeWorkCr',
                    text: 'Вид работы',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'PaymentType',
                    text: 'Вид оплаты',
                    flex: 1,
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ActPaymentType',
                    editor: {
                        xtype: 'b4enumcombo',
                        enumName: 'B4.enums.ActPaymentType'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePayment',
                    flex: 1,
                    text: 'Дата оплаты',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'numbercolumn',
                    format: '0,00',
                    dataIndex: 'ActSum',
                    text: 'Сумма по распоряжению, руб',
                    flex: 2
                },
                {
                    dataIndex: 'PaymentSum',
                    text: 'Оплачено, руб',
                    flex: 1,
                    editor: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        decimalSeparator: ','
                    }
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
                                    xtype: 'button',
                                    action: 'acceptDistribution',
                                    text: 'Применить распределение'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('window').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Cr',
                            fieldLabel: 'Остаток',
                            value: me.sum,
                            readOnly: true,
                            margin: '1 0 1 7',
                            labelWidth: 115,
                            maxLength: 300
                        }
                    ]
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEdit'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});