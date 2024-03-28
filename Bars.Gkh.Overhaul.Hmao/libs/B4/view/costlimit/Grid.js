Ext.define('B4.view.costlimit.Grid',
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

        alias: 'widget.costlimitgrid',
        title: 'Предельная стоимость услуг или работ',
        store: 'CostLimit',
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
                            header: 'Категория',
                            dataIndex: 'CapitalGroup',
                            filter: { xtype: 'textfield' },
                            sortable: true,
                            flex: 3,
                        },
                        {
                            header: 'Работа',
                            dataIndex: 'Work',
                            filter: { xtype: 'textfield' },
                            sortable: true,
                            flex: 5,
                        },
                        //{
                        //    header: 'МО',
                        //    dataIndex: 'Municipality',
                        //    filter: { xtype: 'textfield' },
                        //    sortable: true,
                        //    flex: 5,
                        //},
                        {
                            header: 'Год',
                            dataIndex: 'Year',
                            filter: { xtype: 'numberfield' },
                            sortable: true,
                            flex: 3,
                        },
                        {
                            header: 'Индекс инфляции',
                            dataIndex: 'Rate',
                            filter: { xtype: 'numberfield' },
                            sortable: true,
                            flex: 3,
                        },
                        {
                            header: 'Средняя стоимость',
                            dataIndex: 'Cost',
                            filter: { xtype: 'numberfield' },
                            sortable: true,
                            flex: 5,
                        },
                        {
                            header: 'Дата начала действия',
                            dataIndex: 'DateStart',
                            format: 'd.m.Y',
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            },
                            sortable: true,
                            flex: 4,
                        },
                        {
                            header: 'Дата окончания действия',
                            dataIndex: 'DateEnd',
                            format: 'd.m.Y',
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            },
                            sortable: true,
                            flex: 4,
                        },
                        //{
                        //    header: 'Этаж от',
                        //    dataIndex: 'FloorStart',
                        //    filter: { xtype: 'numberfield' },
                        //    sortable: true,
                        //    flex: 3,
                        //},
                        //{
                        //    header: 'Этаж до',
                        //    dataIndex: 'FloorEnd',
                        //    filter: { xtype: 'numberfield' },
                        //    sortable: true,
                        //    flex: 3,
                        //},
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
                                        {
                                            xtype: 'button',
                                            text: 'Расчет',
                                            name: 'calculateButton',
                                            tooltip: 'Рассчитать предельные стоимости',
                                            action: 'ProcessCalculation',
                                            iconCls: 'icon-accept',
                                            itemId: 'ProcessCalculation'
                                        }
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