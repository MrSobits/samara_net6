Ext.define('B4.view.transferfunds.HireGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.feature.GroupingSummaryTotal',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.view.Control.GkhDecimalField',
        'B4.store.transferfunds.Hire',
        'B4.ux.grid.filter.YesNo',
        'B4.grid.feature.Summary'
    ],
    alias: 'widget.transferfundshiregrid',
    title: 'Перечисления по найму',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.transferfunds.Hire'),
            numberRenderer = function(val) {
                return Ext.util.Format.currency(val);
            };

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    },
                    summaryRenderer: function() {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 2.7,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountNum',
                    flex: 1,
                    text: 'Номер ЛС',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'PaidTotal',
                    text: 'Оплачено',
                    flex: 1,
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'TransferredSum',
                    text: 'Перечисленная сумма',
                    flex: 1,
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    editor: { xtype: 'gkhdecimalfield' },
                    renderer: numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Transferred',
                    flex: 1,
                    text: 'Перечислено',
                    renderer: function(val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' },
                    editor: {
                        xtype: 'checkbox'
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'BeforeTransfer',
                    text: 'Перечисления в текущем периоде',
                    flex: 1.5,
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: numberRenderer
                }
            ],
            //selType: 'cellmodel',
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        beforeedit: function(editor, e) {
                            if (e.field == 'Transferred' && e.record.get('PaidTotal') === 0) {
                                return false;
                            }
                            
                            return true;
                        },
                        edit: function(editor, e) {
                            if (e.field == 'Transferred') {
                                if (e.value === false) {
                                    e.record.set('TransferredSum', 0);
                                } else {
                                    e.record.set('TransferredSum', e.record.get('PaidTotal'));
                                }
                            }
                        }
                    }
                })
            ],
            features: [
                { ftype: 'summary' }
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
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Обновить',
                                    tooltip: 'Обновить',
                                    iconCls: 'icon-arrow-refresh',
                                    listeners: {
                                        click: function() {
                                            store.load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    action: 'SaveObjects',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'button',
                                    action: 'Calc',
                                    iconCls: 'icon-accept',
                                    text: 'Расчет'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});