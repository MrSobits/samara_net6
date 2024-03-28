Ext.define('B4.view.objectcr.scheduleexecutionwork.AddDateGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.schexworkdategrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.button.Close',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Дополнительный срок',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.ScheduleExecutionWork');

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
                    xtype: 'datecolumn',
                    dataIndex: 'ControlDate',
                    text: 'Контрольный срок',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AdditionalDate',
                    text: 'Дополнительный срок',
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
                                    itemId: 'fillDateButton',
                                    text: 'Выставить доп.срок',
                                    iconCls: 'icon-arrow-in'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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