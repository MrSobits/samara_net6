Ext.define('B4.view.dict.effectivenessandperformanceindex.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.service.ContragentForProvider'
    ],

    title: 'Показатели эффективности и результативности',
    store: 'dict.EffectivenessAndPerformanceIndex',
    alias: 'widget.effectivenessandperformanceindexgrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    filter: { xtype: 'textfield'},
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 6,
                    filter: { xtype: 'textfield' },
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ParameterName',
                    flex: 3,
                    filter: { xtype: 'textfield' },
                    text: 'Наименование параметра',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 3,
                    filter: { xtype: 'textfield' },
                    text: 'Единица измерения параметра',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300,
                        allowBlank: false
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
                    dock: 'bottom',
                    displayInfo: true,
                    store: this.store
                }
            ]
        });

        me.callParent(arguments);
    }
});