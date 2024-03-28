Ext.define('B4.view.paysize.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paysizegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.Paysize',
        'B4.enums.TypeIndicator',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Размеры взносов на КР',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.Paysize', {
                autoLoad: false
            });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeIndicator',
                    filter: true,
                    dataIndex: 'Indicator',
                    text: 'Показатель',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    text: 'Начало периода',
                    dataIndex: 'DateStart',
                    flex: 1,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Окончание периода',
                    dataIndex: 'DateEnd',
                    flex: 1,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
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
                            items: [
                                { xtype: 'b4addbutton' },
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    action: 'UpdateRoTypes',
                                    iconCls: 'icon-table-go',
                                    text: 'Обновить типы домов'
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