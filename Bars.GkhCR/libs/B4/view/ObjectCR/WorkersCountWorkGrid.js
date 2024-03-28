Ext.define('B4.view.objectcr.WorkersCountWorkGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField',
        'B4.store.objectcr.WorkersCountWork'
    ],

    title: 'Численность рабочих',
    alias: 'widget.objectcrworkersgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.WorkersCountWork');
        
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkName',
                    flex: 2,
                    text: 'Вид работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FinanceSourceName',
                    flex: 1,
                    text: 'Разрез финансирования'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasureName',
                    width: 80,
                    text: 'Ед. изм.'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    text: 'Плановый объем',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'CountWorker',
                    text: 'Количество работников',
                    flex: 1,
                    editor: 'gkhdecimalfield',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
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