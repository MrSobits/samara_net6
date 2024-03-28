Ext.define('B4.view.suspenseaccount.RealObjDistributionGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.realobjdistributiongrid',
    requires: [
        'B4.form.ComboBox',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Close'
    ],

    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            store: Ext.create('Ext.data.Store', {
                fields: [
                    { name: 'Id' },
                    { name: 'Municipality' },
                    { name: 'Address' },
                    { name: 'LimitSum' },
                    { name: 'Sum' },
                    { name: 'RoId' },
                    { name: 'ObjectId' }
                ]
            }),
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Муниципальное образование',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 3
                },
                {
                    xtype: 'numbercolumn',
                    format: '0,00',
                    dataIndex: 'LimitSum',
                    text: 'Лимит по источнику',
                    flex: 1,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'numbercolumn',
                    format: '0,00',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    flex: 1,
                    editor: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        decimalSeparator: ','
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
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