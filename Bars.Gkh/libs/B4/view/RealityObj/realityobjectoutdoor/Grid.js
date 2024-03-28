Ext.define('B4.view.realityobj.realityobjectoutdoor.Grid',
    {
        extend: 'B4.ux.grid.Panel',
        alias: 'widget.realityobjectoutdoorgrid',

        requires: [
            'B4.ux.button.Add',
            'B4.ux.button.Update',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.ux.grid.toolbar.Paging',
            'B4.ux.grid.column.Delete',
            'B4.ux.grid.column.Edit',
            'B4.store.realityobj.RealityObjectOutdoor'
        ],

        title: 'Реестр дворов',
        closable: true,


        initComponent: function () {
            var me = this,
                store = Ext.create('B4.store.realityobj.RealityObjectOutdoor');
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
                        dataIndex: 'Municipality',
                        flex: 1,
                        text: 'Муниципальное образование',
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
                        dataIndex: 'Locality',
                        flex: 1,
                        text: 'Населенный пункт',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Name',
                        flex: 1,
                        text: 'Наименование двора',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Code',
                        flex: 1,
                        text: 'Код двора',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'RealityObjects',
                        flex: 1,
                        text: 'Жилые дома двора',
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
                                columns: 3,
                                items: [
                                    {
                                        xtype: 'b4addbutton'
                                    },
                                    {
                                        xtype: 'b4updatebutton'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'icon-table-go',
                                        text: 'Выгрузка в Excel',
                                        textAlign: 'left',
                                        action: 'export'
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