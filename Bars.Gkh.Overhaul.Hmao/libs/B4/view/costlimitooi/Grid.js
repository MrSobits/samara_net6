Ext.define('B4.view.costlimitooi.Grid',
    {
        extend: 'B4.ux.grid.Panel',
        requires: [
            'B4.ux.button.Update',
            'B4.ux.button.Add',
            'B4.ux.grid.column.Edit',
            'B4.ux.grid.column.Delete',
            'B4.ux.grid.toolbar.Paging',
            'B4.ux.grid.plugin.HeaderFilters'
        ],

        alias: 'widget.costlimitgridooi',
        title: 'Предельная стоимость услуг или работ',
        store: 'CostLimitOOI',
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
                            header: 'Id',
                            dataIndex: 'Id',
                            filter: { xtype: 'numberfield' },
                            sortable: true,
                            flex: 1,
                        },
                        {
                            header: 'ООИ',
                            dataIndex: 'CommonEstateObject',
                            filter: { xtype: 'textfield' },
                            sortable: true,
                            flex: 5,
                        },
                        {
                            header: 'МО',
                            dataIndex: 'Municipality',
                            filter: { xtype: 'textfield' },
                            sortable: true,
                            flex: 5,
                        },
                        {
                            header: 'Максимальная стоимость',
                            dataIndex: 'Cost',
                            filter: { xtype: 'numberfield' },
                            sortable: true,
                            flex: 5,
                        },
                        {
                            header: 'Дата начала действия',
                            dataIndex: 'DateStart',
                            filter: { xtype: 'datefield' },
                            sortable: true,
                            flex: 4,
                        },
                        {
                            header: 'Дата окончания действия',
                            dataIndex: 'DateEnd',
                            filter: { xtype: 'datefield' },
                            sortable: true,
                            flex: 4,
                        },
                        {
                            header: 'Этаж от',
                            dataIndex: 'FloorStart',
                            filter: { xtype: 'numberfield' },
                            sortable: true,
                            flex: 3,
                        },
                        {
                            header: 'Этаж до',
                            dataIndex: 'FloorEnd',
                            filter: { xtype: 'numberfield' },
                            sortable: true,
                            flex: 3,
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