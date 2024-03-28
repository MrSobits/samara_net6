Ext.define('B4.view.frgufunction.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',

        'B4.ux.grid.toolbar.Paging',
    ],

    title: 'Функции из ФРГУ',
    alias: 'widget.frgufunctionGrid',
    closable: false,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.FrguFunction');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование контрольно-надзорной функции из ФРГУ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FrguId',
                    flex: 1,
                    text: 'Идентификатор контрольно-надзорной функции из ФРГУ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Guid',
                    flex: 1,
                    text: 'Идентификатор контрольно-надзорной функции формата GUID'
                },
                {
                    xtype: 'b4deletecolumn'
                }
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
                            items: [
                                {
                                    xtype: 'b4addbutton'
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