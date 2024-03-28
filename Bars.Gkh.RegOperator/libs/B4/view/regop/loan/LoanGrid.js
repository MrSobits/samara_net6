Ext.define('B4.view.regop.loan.LoanGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.GridStateColumn',
        'B4.form.ComboBox',
        'B4.store.regop.Loan',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Реестр займов',

    alias: 'widget.loangrid',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.Loan'),
            renderer = function (val) {
                val = val || 0;
                return Ext.util.Format.currency(val);
            };

        Ext.apply(me, {
            store: store,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            cls: 'x-large-head',
            columns: [
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function(field, s, options) {
                                options.params.typeId = 'gkh_regop_reality_object_loan';
                            },
                            storeloaded: {
                                fn: function(cmb) {
                                    cmb.getStore().insert(0, { Id: null, Name: '-' });
                                    cmb.select(cmb.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        var record;
                        if (type == 'click' && e.target.localName == 'img') {
                            record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LoanDate',
                    text: 'Дата займа',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    flex: 1
                },
                {
                    text: 'Источники займа',
                    dataIndex: 'Sources',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Краткосрочный план',
                    dataIndex: 'ProgramCr',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    dataIndex: 'Settlement',
                    itemId: 'SettlementColumn',
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Заниматель',
                    dataIndex: 'LoanReceiver',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    text: 'Сумма займа (руб.)',
                    dataIndex: 'LoanSum',
                    flex: 1,
                    renderer: renderer
                },
                {
                    text: 'Погашено (руб.)',
                    dataIndex: 'LoanReturnedSum',
                    flex: 1,
                    renderer: renderer
                },
                {
                    text: 'Задолженность (руб.)',
                    dataIndex: 'DebtSum',
                    flex: 1,
                    renderer: renderer
                },
                {
                    text: 'Сальдо (руб.)',
                    dataIndex: 'Saldo',
                    flex: 1,
                    renderer: renderer
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'FactEndDate',
                    text: 'Фактическая дата возврата',
                    format: 'd.m.Y',
                    flex: 1
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
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function(btn) {
                                            btn.up('b4grid').getStore().load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    name: 'repaymentAll',
                                    text: 'Осуществить возврат займов'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
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
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true,
                markDirty: false
            }
        });

        me.callParent(arguments);

    }
});