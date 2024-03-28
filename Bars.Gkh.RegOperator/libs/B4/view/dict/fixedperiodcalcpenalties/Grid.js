Ext.define('B4.view.dict.fixedperiodcalcpenalties.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.toolbar.Paging'
    ],

    store: 'dict.FixedPeriodCalcPenalties',
    alias: 'widget.fixedperiodcalcpenaltiesGrid',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'StartDay',
                    text: 'День начала периода',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EndDay',
                    text: 'День окончания периода',
                    flex: 1
                },

                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Действует с',
                    flex: 1,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Действует по',
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