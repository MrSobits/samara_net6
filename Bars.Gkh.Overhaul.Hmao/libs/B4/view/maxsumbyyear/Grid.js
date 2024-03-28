Ext.define('B4.view.maxsumbyyear.Grid',
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

        alias: 'widget.maxsumbyyeargrid',
        title: 'Предельные стоимости в разрезе МО',
        store: 'MaxSumByYear',
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
                        {
                            header: 'Муниципальное образование',
                            displayField: 'Name',
                            dataIndex: 'Municipality',
                            flex: 5,
                            filter: { xtype: 'textfield' },
                            sortable: false
                        },
                        {
                            header: 'Программа ДПКР',
                            displayField: 'Name',
                            dataIndex: 'Program',
                            flex: 5,
                            filter: { xtype: 'textfield' },
                            sortable: false
                        },
                        {
                            header: 'Год',
                            dataIndex: 'Year',
                            flex: 5,
                            filter: { xtype: 'textfield' },
                            sortable: false
                        },
                        {
                            header: 'Максимальная стоимость',
                            dataIndex: 'Sum',
                            flex: 5,
                            filter: { xtype: 'textfield' },
                            sortable: false
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