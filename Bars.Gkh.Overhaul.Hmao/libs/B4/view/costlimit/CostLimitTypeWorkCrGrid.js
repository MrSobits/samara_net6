Ext.define('B4.view.costlimit.CostLimitTypeWorkCrGrid',
    {
        extend: 'B4.ux.grid.Panel',
        requires: [
            'B4.ux.button.Update',
            'B4.ux.button.Add',
            'B4.ux.grid.toolbar.Paging',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.ux.grid.column.Delete'
        ],

        alias: 'widget.costLimitTypeWorkCrGrid',
        store: 'CostLimitTypeWorkCr',
        initComponent: function () {
            var me = this;
            Ext.applyIf(me,
                {
                    columnLines: true,
                    defaults: {
                        sortable: true,
                        editable: false,
                    },
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Address',
                            text: 'Адрес',
                            filter: { xtype: 'textfield' },
                            flex: 2
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Cost',
                            text: 'Стоимость',
                            filter: { xtype: 'numberfield' },
                            allowDecimals: true,
                            flex: 1
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Volume',
                            text: 'Объем',
                            filter: { xtype: 'numberfield' },
                            allowDecimals: true,
                            flex: 1
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'UnitCost',
                            text: 'Удельная стоимость',
                            filter: { xtype: 'numberfield' },
                            allowDecimals: true,
                            flex: 1
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Year',
                            text: 'Год',
                            filter: { xtype: 'textfield' },
                            flex: 1
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'UnitMeasure',
                            text: 'Ед.измерения',
                            filter: { xtype: 'textfield' },
                            flex: 1
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
                                            xtype: 'b4updatebutton'
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