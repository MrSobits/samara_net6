Ext.define('B4.view.longtermprobject.specialaccount.OperationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.specaccountopergrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.account.operation.Special',
        'B4.store.dict.AccountOperationNoPaging',
        'B4.form.ComboBox'
    ],

    title: 'Операции по специальному счету',
    store: 'account.operation.Special',

    initComponent: function() {
        var me = this,
            storeOperations = Ext.create('B4.store.dict.AccountOperationNoPaging', {
                remoteFilter: false
            }),
            numberfield = {
                xtype: 'numberfield',
                allowDecimals: true,
                decimalSeparator: ',',
                hideTrigger: true
            };

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
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    },
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма',
                    editor: numberfield,
                    filter: numberfield
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Receiver',
                    flex: 1,
                    text: 'Получатель',
                    filter: { xtype: 'textfield' },
                    editor: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Payer',
                    flex: 1,
                    text: 'Плательщик',
                    filter: { xtype: 'textfield' },
                    editor: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Purpose',
                    flex: 1,
                    text: 'Назначение',
                    filter: { xtype: 'textfield' },
                    editor: { xtype: 'textfield' }
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