Ext.define('B4.view.dict.penaltieswithdeferred.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.toolbar.Paging'
    ],

    store: 'dict.PenaltiesWithDeferred',
    alias: 'widget.penaltiesdeferredGrid',
    closable: false,

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
                    xtype: 'datecolumn',
                    dataIndex: 'DateStartCalc',
                    text: 'Дата начала расчета',
                    flex: 1,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEndCalc',
                    text: 'Дата окончания расчета',
                    flex: 1,
                    format: 'd.m.Y'
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});