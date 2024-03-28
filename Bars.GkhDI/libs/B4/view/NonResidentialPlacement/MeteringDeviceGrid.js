Ext.define('B4.view.nonresidentialplacement.MeteringDeviceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.meterdevicegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.TypeAccounting'
    ],
    store: 'nonresidentialplacement.MeteringDevice',
    itemId: 'nonResidentialPlacementMeteringDeviceGrid',
    title: 'Приборы учета',
    layout: 'fit',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование прибора'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccuracyClass',
                    text: 'Класс точности',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeAccounting',
                    flex: 1,
                    text: 'Тип учета',
                    renderer: function (val) { return B4.enums.TypeAccounting.displayRenderer(val); }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});