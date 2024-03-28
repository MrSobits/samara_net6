Ext.define('B4.view.dict.efficiencyratingperiod.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo'
    ],

    title: 'Периоды рейтинга эффективности',
    alias: 'widget.efficiencyratingperiodGrid',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.dict.EfficiencyRatingPeriod');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Период',
                    filter: {
                        xtype: 'textfield',
                        maxLength: 255
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 255,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateStart',
                    width: 200,
                    text: 'Дата начала',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'datefield',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateEnd',
                    width: 200,
                    text: 'Дата окончания',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'datefield'
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
                                    xtype: 'b4addbutton'
                                },
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