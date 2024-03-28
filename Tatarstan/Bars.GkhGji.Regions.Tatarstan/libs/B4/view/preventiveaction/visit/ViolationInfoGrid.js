Ext.define('B4.view.preventiveaction.visit.ViolationInfoGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.visitviolationinfogrid',
    store: 'preventiveaction.visit.ViolationInfo',

    itemId: 'visitViolationInfoGrid',
    title: 'Выявленные нарушения',

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
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 0.5,
                    text: 'Адрес'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Violation',
                    flex: 1,
                    text: 'Нарушение'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});