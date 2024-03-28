Ext.define('B4.view.realityobj.realityobjectoutdoor.RealityObjectsInOutdoorGrid',
    {
        extend: 'B4.ux.grid.Panel',
        alias: 'widget.realityobjectsinoutdoorgrid',

        requires: [
            'B4.ux.button.Add',
            'B4.ux.button.Update',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.ux.grid.toolbar.Paging',
            'B4.ux.grid.column.Delete',
            'B4.store.RealityObject',
            'B4.ux.grid.column.Delete',
        ],

        title: 'Жилые дома двора',

        initComponent: function () {
            var me = this,
                store = Ext.create('B4.store.RealityObject');

            Ext.applyIf(me, {
                columnLines: true,
                store: store,
                columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Municipality',
                        flex: 1,
                        text: 'Муниципальное образование',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Address',
                        flex: 1,
                        text: 'Адрес',
                        filter: { xtype: 'textfield' }
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