Ext.define('B4.view.specialobjectcr.ScheduleExecutionWorkGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.specialobjectcrscheduleexecutionworkgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'График выполнения работ',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.specialobjectcr.ScheduleExecutionWork');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkName',
                    flex: 1,
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
                    width: 100,
                    text: 'Ед. изм.'
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
                                },
                                {
                                    xtype: 'button',
                                    name: 'additionalDateButton',
                                    text: 'Дополнительный срок',
                                    iconCls: 'icon-arrow-in'
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