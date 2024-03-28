Ext.define('B4.view.bankstatement.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.view.Control.GkhDecimalField'
    ],

    store: 'BankStatement',
    alias: 'widget.bankStatementGrid',
    title: 'Банковские выписки',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                 {
                     xtype: 'b4editcolumn',
                     scope: me
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetYear',
                    width: 100,
                    text: 'Бюджетный год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    width: 50,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Дата',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingBalance',
                    width: 100,
                    text: 'Вх.остаток',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OutgoingBalance',
                    width: 100,
                    text: 'Исх.остаток',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                     xtype: 'gridcolumn',
                     dataIndex: 'MunicipalityName',
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
                     }
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectCrName',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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