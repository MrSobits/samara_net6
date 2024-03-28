Ext.define('B4.view.objectcr.BuildControlTypeWorkSmrGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],
    title: 'Контроль за ходом работ',
    alias: 'widget.buildcontroltypeworksmrgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.BuildControlTypeWorkSmr');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWorkCrAddWork',
                    text: 'Этап работы',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    text: 'Исполнитель',
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Controller',
                    text: 'Стройконтроль',
                    flex: 2
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectCreateDate',
                    width: 100,
                    format: 'd.m.Y',
                    text: 'Дата создания',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'MonitoringDate',
                    width: 100,
                    format: 'd.m.Y',
                    text: 'Дата контроля',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'VolumeOfCompletion',
                    text: 'Объем выполнения',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PercentOfCompletion',
                    text: 'Процент выполнения',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CostSum',
                    text: 'Сумма расходов',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});