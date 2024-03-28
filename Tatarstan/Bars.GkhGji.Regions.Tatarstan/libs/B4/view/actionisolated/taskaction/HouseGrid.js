Ext.define('B4.view.actionisolated.taskaction.HouseGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.taskactionisolatedhousegrid',
    store: 'actionisolated.taskaction.House',
    itemId: 'taskActionIsolatedHouseGrid',
    title: 'Дома',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me,
            {
                columnLines: true,
                columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Municipality',
                        flex: 1,
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
                        filter: { xtype: 'textfield' },
                        dataIndex: 'Address',
                        text: 'Адрес'
                    },
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'Area',
                        text: 'Площадь'
                    },
                    {
                        xtype: 'b4deletecolumn',
                        hidden: true,
                        scope: me
                    }
                ],
                plugins: [
                    Ext.create('B4.ux.grid.plugin.HeaderFilters')
                ],
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
                        store: me.store,
                        dock: 'bottom'
                    }
                ]
            }
        );

        me.callParent(arguments);
    }
});