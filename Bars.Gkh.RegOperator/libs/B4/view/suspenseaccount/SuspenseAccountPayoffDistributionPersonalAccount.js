Ext.define('B4.view.suspenseaccount.SuspenseAccountPayoffDistributionPersonalAccount', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.grid.feature.Summary',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Import',
        'B4.store.regop.SuspenseAccount',
        'B4.store.regop.personal_account.PersonalAccountOperation',
        'B4.enums.regop.PersonalAccountOwnerType'
    ],

    alias: 'widget.suspenseaccounpayoffdistributionpersonalaccount',
    itemId: 'suspenseaccountGrid',
    header: false,

    initComponent: function() {
        var me = this,
            columns = [
                {
                    text: 'Номер ЛС',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Абонент',
                    dataIndex: 'AccountOwner',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'RoomAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Тип абонента',
                    dataIndex: 'OwnerType',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    renderer: function (value) {
                        try {
                            return B4.enums.regop.PersonalAccountOwnerType.getStore().findRecord('Value', value || 0).get('Display');
                        } catch (e) {
                            return '';
                        }
                    }
                }
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
                    { name: 'PersonalAccountNum', type: 'string' },
                    { name: 'AccountOwner', type: 'string' },
                    { name: 'RoomAddress', type: 'string' },
                    { name: 'OwnerType', type: 'string' },
                    { name: 'Sum', type: 'string' },
                    { name: 'SumPenalty', type: 'string' }
                ]
            }),
            columns: columns,
            features: [{
                ftype: 'b4_summary'
            }],
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
                                    iconCls: 'icon-accept',
                                    text: 'Применить распределение'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'Cr',
                                    fieldLabel: 'Остаток',
                                    value: me.sum,
                                    readOnly: true,
                                    margin: '1 0 1 7',
                                    labelAlign: 'right',
                                    labelWidth: 40,
                                    maxLength: 200
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
                                        click: function(btn) {
                                            btn.up('window').close();
                                        }
                                    }
                                }
                            ]
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