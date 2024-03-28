Ext.define('B4.view.gkuinfo.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.store.GkuInfo',
        'B4.view.Control.GkhDecimalField'
    ],

    title: 'Информация о ЖКУ',
    alias: 'widget.gkuinfogrid',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.GkuInfo');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 200,
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
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MonthCharge',
                    flex: 1,
                    text: 'Начислено',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Paid',
                    flex: 1,
                    text: 'Оплачено',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Debt',
                    flex: 1,
                    text: 'Задолженность',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Collection',
                    flex: 1,
                    text: 'Собираемость',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'showIndividual',
                                    boxLabel: 'Показать индивидуальные дома',
                                    labelWidth: 160
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'showBlocked',
                                    boxLabel: 'Показать блокир. застройки',
                                    labelWidth: 150
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