Ext.define('B4.view.controllist.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.enums.KindKNDGJI',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Проверочные листы',
    store: 'dict.ControlList',
    alias: 'widget.controllistgrid',
    closable: true,

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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.KindKNDGJI',
                    dataIndex: 'KindKNDGJI',
                    text: 'Вид КНД',
                    flex: 0.5,
                    filter: true
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'Code',
                     flex: 0.5,
                     text: 'Номер',
                     filter: {
                         xtype: 'textfield'
                     }
                 },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'Name',
                     flex: 2,
                     text: 'Наименование',
                     filter: {
                         xtype: 'textfield'
                     }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ERKNMGuid',
                    flex: 1,
                    text: 'ЕРКНМ Guid'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ActualFrom',
                    text: 'Дата с',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ActualTo',
                    text: 'Дата по',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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