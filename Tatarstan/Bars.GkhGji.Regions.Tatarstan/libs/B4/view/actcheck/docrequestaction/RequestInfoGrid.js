Ext.define('B4.view.actcheck.docrequestaction.RequestInfoGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.view.Control.GkhFileColumn',
        'B4.enums.RequestInfoType'
    ],

    alias: 'widget.docrequestactionrequestinfogrid',
    itemId: 'docRequestActionRequestInfoGrid',
    title: 'Запрошенные сведения',
    
    store: 'actcheck.DocRequestActionRequestInfo',

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
                    enumName: 'B4.enums.RequestInfoType',
                    dataIndex: 'RequestInfoType',
                    flex: 1,
                    text: 'Описание'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'gkhfilecolumn',
                    dataIndex: 'File',
                    width: 100
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            viewConfig: {
                loadMask: true
            },
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
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
                }
            ]
        });

        me.callParent(arguments);
    }
});