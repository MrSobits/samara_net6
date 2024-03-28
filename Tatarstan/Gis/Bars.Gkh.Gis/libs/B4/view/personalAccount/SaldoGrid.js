/*
    Сальдо по лицевому счету - Grid
*/
Ext.define('B4.view.personalAccount.SaldoGrid', {
    extend: 'Ext.grid.Panel',
    
    alias: 'widget.personalAccount_saldo_grid',
    
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Edit',
        'B4.store.PersonalAccountSaldo'
    ],

    title: 'Сальдо по лицевому счету',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.PersonalAccountSaldo');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            enableColumnHide: true,
            columns: [
                //{
                //    xtype: 'b4editcolumn',
                //    text: '',
                //    tooltip: 'Вывести протокол'
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    flex: 3,
                    text: 'Услуга',
                    groupByColumn: true,
                    summaryRenderer: function (value, summary, dataIndex) {
                        var firstVisibleColumn = me.getFirstVisibleColumn(this);

                        if (firstVisibleColumn.dataIndex == dataIndex) {
                            return "<b>Всего:</b>";
                        }
                    },

                    listeners: {
                        //Если колонка была скрыта, а теперь видна, то перегружаем стор
                        beforeshow: function (column) {
                            if (column.hidden) {
                                column.groupByColumn = true;
                                column.up('gridpanel').getStore().load();
                            }
                        },
                        //Аналогично со скрытием колонки
                        beforehide: function (column) {
                            if (!column.hidden) {
                                column.groupByColumn = false;
                                column.up('gridpanel').getStore().load();
                            }
                        }
                    },

                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Supplier',
                    hidden: true,
                    flex: 3,
                    text: 'Агентский договор',
                    groupByColumn: false,
                    summaryRenderer: function (value, summary, dataIndex) {
                        var firstVisibleColumn = me.getFirstVisibleColumn(this);

                        if (firstVisibleColumn.dataIndex == dataIndex) {
                            return "<b>Всего:</b>";
                        }
                    },
                    listeners: {
                        //Если колонка была скрыта, а теперь видна, то перегружаем стор
                        beforeshow: function (column) {
                            if (column.hidden) {
                                column.groupByColumn = true;
                                column.up('gridpanel').getStore().load();
                            }
                        },
                        //Аналогично со скрытием колонки
                        beforehide: function (column) {
                            if (!column.hidden) {
                                column.groupByColumn = false;
                                column.up('gridpanel').getStore().load();
                            }
                        }
                    },

                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'IncomingSaldo',
                    flex: 1,
                    text: 'Входящее сальдо',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'Credited',
                    flex: 1,
                    text: 'Начислено',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'Recalculation',
                    flex: 1,
                    text: 'Перерасчет',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'Change',
                    flex: 1,
                    text: 'Изменения',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'Paid',
                    flex: 1,
                    text: 'Оплачено',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'OutcomingSaldo',
                    flex: 1,
                    text: 'Исходящее сальдо',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'Payable',
                    flex: 1,
                    text: 'К оплате',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'Distributed',
                    flex: 1,
                    text: 'Распределено',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    hideable: false,
                    dataIndex: 'CurrentDebt',
                    flex: 1,
                    text: 'Текущий долг',
                    renderer: me.numberRenderer,
                    summaryType: 'sum',
                    summaryRenderer: me.numberRenderer
                }
            ],

            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters', { pluginId: 'filter' })
            ],

            dockedItems:
            [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            defaults: {
                                margin: '2 0 2 0'
                            },
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    name: 'refreshList',
                                    text: 'Обновить',
                                    tooltip: 'Обновить',
                                    iconCls: 'icon-arrow-refresh'
                                }
                                //{
                                //    xtype: 'button',
                                //    name: 'calculate',
                                //    text: 'Расчет',
                                //    tooltip: 'Расчет',
                                //    iconCls: 'icon-calculator'
                                //}
                            ]
                        }
                    ]
                }
            ],
            viewConfig: {
                loadMask: true
            },
            features: [
                {
                    ftype: 'summary'
                }
            ]
        });


        me.callParent(arguments);
    },

    numberRenderer: function (value) {
        return Ext.util.Format.number(parseFloat(value), '0,000.00').replace(',', '.');
    },

    getFirstVisibleColumn: function (summary) {
        var grid = summary.grid;
        for (var i in grid.columns) {
            if (!grid.columns[i].hidden)
                return grid.columns[i];
        }
    }
    

});