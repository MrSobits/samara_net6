Ext.define('B4.view.dict.service.BilServiceGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Связанные услуги',
    alias: 'widget.bilservicedictgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.service.BilServiceDictionary');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'DataBank',
                        flex: 1,
                        text: 'Банк данных',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Organization',
                        flex: 1,
                        text: 'Организация',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'ServiceName',
                        flex: 1,
                        text: 'Услуга',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'ServiceCode',
                        flex: 1,
                        text: 'Код услуги',
                        filter: {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            minValue: 1,
                            operand: CondExpr.operands.eq,
                            allowDecimals: false
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'ServiceTypeName',
                        flex: 1,
                        text: 'Тип услуги',
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
                                    xtype: 'button',
                                    itemId: 'addBillServRef',
                                    iconCls: 'icon-add',
                                    text: 'Связать услуги',
                                    actionName: 'add'
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