Ext.define('B4.view.longtermprobject.realaccount.OperationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realaccountopergrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    title: 'Операции по реальному счету',
    store: 'account.operation.Real',

    initComponent: function() {
        var me = this,
            storeOperations = Ext.create('B4.store.dict.AccountOperationNoPaging', {
                remoteFilter: false
            });

        storeOperations.load();

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Operation',
                    flex: 1,
                    text: 'Операция',
                    editor: {
                        xtype: 'b4combobox',
                        store: storeOperations,
                        displayField: 'Name',
                        valueField: 'Id',
                        editable: false
                    },
                    renderer: function (val) {
                        var ac;
                        if (val) {
                            ac = storeOperations.findRecord('Id', val, 0, false, false, true);
                            if (ac) {
                                return ac.get('Name');
                            }
                        }
                        return '';
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OperationDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100,
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        decimalSeparator: ',',
                        allowDecimals: true
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        decimalSeparator: ',',
                        allowDecimals: true
                    },
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Receiver',
                    flex: 1,
                    text: 'Получатель',
                    editor: {
                        xtype: 'textfield'
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Payer',
                    flex: 1,
                    text: 'Плательщик',
                    editor: {
                        xtype: 'textfield'
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Purpose',
                    flex: 1,
                    text: 'Назначение',
                    editor: {
                        xtype: 'textfield'
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'btnSave',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});