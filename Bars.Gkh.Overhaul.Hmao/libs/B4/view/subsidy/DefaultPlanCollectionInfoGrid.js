Ext.define('B4.view.subsidy.DefaultPlanCollectionInfoGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.defaultplancollectioninfogrid',
    requires: [
        'B4.store.subsidy.DefaultPlanCollectionInfo',
        'B4.ux.button.Update',
        'B4.ux.button.Save',

        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Плановые показатели собираемости',

    store: 'subsidy.DefaultPlanCollectionInfo',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.subsidy.DefaultPlanCollectionInfo');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    flex: 1,
                    sortable: false,
                    text: 'Год',
                    width: 80
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanOwnerPercent',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Плановая собираемость, %',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2,
                        minValue: 0,
                        maxValue: 100
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NotReduceSizePercent',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Неснижаемый размер регионального фонда, %',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        decimalPrecision: 2,
                        minValue: 0,
                        maxValue: 100
                    }
                }              
            ],
            plugins: [
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
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить период',
                                    textAlign: 'left',
                                    action: 'UpdatePeriod'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});