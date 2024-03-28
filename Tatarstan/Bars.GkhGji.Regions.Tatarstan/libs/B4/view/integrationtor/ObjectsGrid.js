Ext.define('B4.view.integrationtor.ObjectsGrid',
    {
        extend: 'B4.ux.grid.Panel',
        alias: 'widget.integrationtorobjectsgrid',

        requires: [
            'B4.ux.button.Update',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.ux.grid.toolbar.Paging',
            'B4.store.integrationtor.Object'
        ],

        title: 'Отправленные объекты',
        closable: true,

        initComponent: function () {
            var me = this,
                store = Ext.create('B4.store.integrationtor.Object');
            Ext.applyIf(me, {
                columnLines: true,
                store: store,
                columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'TorId',
                        text: 'Внешний ID',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Municipality',
                        text: 'Муниципальный район',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Address',
                        text: 'Адрес',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'FiasAddress',
                        text: 'Код ФИАС',
                        flex: 1,
                        filter: { xtype: 'textfield' }
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
                        items: [
                            {
                                xtype: 'buttongroup',
                                columns: 2,
                                items: [
                                    {
                                        xtype: 'b4updatebutton',
                                        listeners: {
                                            click: function () {
                                                store.load();
                                            }
                                        }
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