Ext.define('B4.view.assberbank.Grid',
    {
        extend: 'B4.ux.grid.Panel',
        requires: [
            'B4.ux.button.Update',
            'B4.ux.button.Add',
            'B4.ux.grid.column.Edit',
            'B4.ux.grid.column.Delete',
            'B4.ux.grid.toolbar.Paging',
            'B4.ux.grid.plugin.HeaderFilters',
            'Ext.ux.RowExpander',
        ],
        store: 'ASSberbankClient',
        alias: 'widget.assberbankgrid',
        title: 'Настройки выгрузки в Клиент-Сбербанк',
        initComponent: function () {
            var me = this;

            Ext.applyIf(me,
                {
                    store: this.store,
                    columnLines: true,
                    columns: [
                        {
                            xtype: 'b4editcolumn',
                            scope: me
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ClientCode',
                            flex: 1,
                            text: 'Код клиента'
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