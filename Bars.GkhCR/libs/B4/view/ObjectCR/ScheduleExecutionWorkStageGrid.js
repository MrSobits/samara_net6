Ext.define('B4.view.objectcr.ScheduleExecutionWorkStageGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.scheduleexecutionworkstagegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'График выполнения этапов',
    closable: true,
    store: 'objectcr.TypeWorkCrAddWork',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AdditWorkName',
                    flex: 2,
                    text: 'Этап работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Queue',
                    flex: 1,
                    text: 'Очередность',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        maxLength: 2,
                        mouseWheelEnabled: false
                    }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'Required',
                    text: 'Отслеживает СК',
                    flex: 1,
                    sortable: false,
                    editor: {
                        xtype: 'checkbox'
                    },
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStartWork',
                    text: 'Начало работ',
                    format: 'd.m.Y',
                    width: 100,
                    editor: 'datefield'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEndWork',
                    text: 'Окончание работ',
                    format: 'd.m.Y',
                    width: 100,
                    editor: 'datefield'
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
                                { xtype: 'b4addbutton' },
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});