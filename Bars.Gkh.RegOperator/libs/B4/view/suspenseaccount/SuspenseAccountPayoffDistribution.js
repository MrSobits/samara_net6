Ext.define('B4.view.suspenseaccount.SuspenseAccountPayoffDistribution', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Close',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Import',
        'B4.store.regop.SuspenseAccount',
        'B4.store.regop.realty.RealtyPaymentAccount',
        'B4.store.dict.ProgramCr',
        'B4.store.view.ViewRealityObject',
        'B4.store.BasePropertyOwnerDecision'
    ],

    alias: 'widget.suspenseaccounpayoffdistribution',
    header: false,

    initComponent: function() {
        var me = this,
            columns = [
                //{
                //    text: 'Способ накопления средств',
                //    dataIndex: 'MethodFormFund',
                //    flex: 1,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    text: 'Адрес дома',
                    dataIndex: 'Address',
                    name: 'Address',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
                //    ,
                //    {
                //        text: 'Недостаток средств',
                //        dataIndex: 'FundDeficiency',
                //        flex: 1,
                //        filter: { xtype: 'textfield' }
                //    }
            ];

        if (me.enableCellEdit) {
            columns.push({
                text: 'Распределенная сумма',
                dataIndex: 'Sum',
                flex: 1,
                filter: { xtype: 'textfield' },
                editor: {
                    xtype: 'numberfield',
                    minValue: 0,
                    decimalPrecision: 2,
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false
                }
            });
        } else {
            columns.push({
                text: 'Распределенная сумма',
                dataIndex: 'Sum',
                flex: 1,
                filter: { xtype: 'textfield' }
            });
        }


        Ext.apply(me, {
            store: Ext.create('Ext.data.Store', {
                fields: [
                    { name: 'Id', type: 'string' },
                    { name: 'MethodFormFund', type: 'string' },
                    { name: 'Address', type: 'string' },
                    { name: 'FundDeficiency', type: 'string' },
                    { name: 'OwnerType', type: 'string' },
                    { name: 'Sum', type: 'string' }
                ]
            }),
            columns: columns,

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Применить распределение',
                                    action: 'acceptDistribution'
                                }
                            ]
                        }, '->', {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function(btn) {
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
                            xtype: 'textfield',
                            name: 'Cr',
                            fieldLabel: 'Остаток',
                            value: me.sum,
                            margins: '1 0 1 0',
                            maxLength: 300,
                            readOnly: true
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