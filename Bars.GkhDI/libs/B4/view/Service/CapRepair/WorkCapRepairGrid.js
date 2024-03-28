Ext.define('B4.view.service.caprepair.WorkCapRepairGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.workcaprepgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField'
    ],
    store: 'service.WorkCapRepair',
    itemId: 'workCapRepairGrid',
    title: 'Работы',
    closable: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlannedVolume',
                    width: 120,
                    text: 'Запланированный объем',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    },
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlannedCost',
                    width: 120,
                    text: 'Запланированная стоимость (руб.)',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    },
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FactedVolume',
                    width: 120,
                    text: 'Фактический объем',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    },
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FactedCost',
                    width: 120,
                    text: 'Фактическая стоимость (руб.)',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    },
                    editor: 'gkhdecimalfield'
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
                })],
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
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    itemId: 'workCapRepairSaveButton'
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