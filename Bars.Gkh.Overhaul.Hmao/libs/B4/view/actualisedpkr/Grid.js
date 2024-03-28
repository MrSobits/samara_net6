Ext.define('B4.view.actualisedpkr.Grid',
    {
        extend: 'B4.ux.grid.Panel',
        requires: [
            'B4.ux.button.Update',
            'B4.ux.button.Add',
            'B4.ux.grid.column.Edit',
            'B4.ux.grid.column.Delete',
            'B4.ux.grid.toolbar.Paging',
            'B4.ux.grid.plugin.HeaderFilters',
            'Ext.ux.RowExpander'
        ],

        alias: 'widget.actualisedpkrgrid',
        title: 'Условия отбора в актуальную программу',
        store: 'DPKRActualCriterias',
        initComponent: function () {
            var me = this;

            Ext.applyIf(me,
                {
                    columnLines: true,
                    columns: [
                        {
                            xtype: 'b4editcolumn',
                            scope: me
                        },
                        //{
                        //    xtype: 'gridcolumn',
                        //    flex: 1,
                        //    filter: { xtype: 'textfield' },
                        //    dataIndex: 'Login',
                        //    text: 'Пользователь'
                        //},
                        {
                            header: 'Условия',
                            dataIndex: 'Text',
                            flex: 5,
                            filter: { xtype: 'textfield' },
                            sortable: false
                        },
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y H:i:s',
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y'
                            },
                            dataIndex: 'DateStart',
                            text: 'Дата начала действия условий'
                        },
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y H:i:s',
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y'
                            },
                            dataIndex: 'DateEnd',
                            text: 'Дата окончания действия условий'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Operator',
                            flex: 1,
                            text: 'Оператор'
                        },
                        {
                            xtype: 'b4deletecolumn',
                            scope: me
                        }
                    ],
                    plugins: [
                        Ext.create('B4.ux.grid.plugin.HeaderFilters')
                    ],
                    viewConfig: {
                        loadMask: true
                    },
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            name: 'buttons',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    columns: 3,
                                    items: [
                                        {
                                            xtype: 'b4addbutton'
                                        },
                                        {
                                            xtype: 'b4updatebutton',
                                            handler: function () {
                                                var me = this;
                                                me.up('grid').getStore().load();
                                            }
                                        },
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