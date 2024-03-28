﻿Ext.define('B4.view.specialobjectcr.BuildContractTypeWorkGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.contracttypeworkspecialcrgrid',
    requires: [
        'B4.grid.feature.Summary',
        
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField',
        'B4.store.specialobjectcr.BuildContractTypeWork'
    ],

    title: 'Виды работ',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.specialobjectcr.BuildContractTypeWork');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWork',
                    flex: 1,
                    text: 'Вид работ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    width: 100,
                    text: 'Сумма (руб.)',
                    editor: { xtype: 'gkhdecimalfield' },
                    summaryType: 'Sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            features: [{
                ftype: 'b4_summary'
            }],
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
                                { xtype: 'b4addbutton' },
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