Ext.define('B4.view.suspenseaccount.credit.DistributionGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.suspacccreditdistributiongrid',
    requires: [
        'B4.form.ComboBox',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Close',
        'B4.base.Store'
    ],

    distrtype: null,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: [
                    { name: 'Id' },
                    { name: 'Account' },
                    { name: 'AccountOwner' },
                    { name: 'Debt' },
                    { name: 'Sum' }
                ],
                autoLoad: false,
                remoteSort: false
            }),
            editor = me.enableCellEdit
                ? {
                    xtype: 'numberfield',
                    decimalSeparator: ',',
                    minValue: 0,
                    hideTrigger: true
                }
                : null;

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    dataIndex: 'Account',
                    text: 'Номер счета',
                    flex: 1
                },
                {
                    dataIndex: 'AccountOwner',
                    text: 'Владелец счета',
                    flex: 1
                },
                {
                    dataIndex: 'Debt',
                    text: me.distrtype === 150 ? 'Сумма основного долга' : 'Сумма долга по процентам',
                    flex: 1,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    flex: 1,
                    editor: editor,
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
                                    action: 'Accept',
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
                            name: 'Rest',
                            fieldLabel: 'Остаток',
                            value: me.sum,
                            readOnly: true,
                            margin: '1 0 1 7',
                            labelWidth: 115
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