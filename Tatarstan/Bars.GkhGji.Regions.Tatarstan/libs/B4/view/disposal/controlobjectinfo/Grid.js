Ext.define('B4.view.disposal.controlobjectinfo.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.disposalcontrolobjectinfogrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Сведения об объектах контроля',
    store: 'disposal.ControlObjectInfo',
    itemId: 'disposalControlObjectInfoGrid',

    initComponent: function() {
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
                    dataIndex: 'InspGjiRealityObject',
                    flex: 1,
                    text: 'Объект контроля'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlObjectType',
                    flex: 1,
                    text: 'Тип объекта'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlObjectKind',
                    flex: 1,
                    text: 'Вид объекта'
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