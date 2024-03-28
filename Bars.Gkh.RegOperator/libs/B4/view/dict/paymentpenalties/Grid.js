Ext.define('B4.view.dict.paymentpenalties.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.enums.CrFundFormationDecisionType',
        'B4.ux.grid.column.Enum'
    ],

    store: 'dict.PaymentPenalties',
    alias: 'widget.paymentpenaltiesGrid',
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
                    xtype: 'b4enumcolumn',
                    dataIndex: 'DecisionType',
                    enumName: 'B4.enums.CrFundFormationDecisionType',
                    text: 'Способ формирования фонда',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Days',
                    text: 'Установленный срок оплаты',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Percentage',
                    text: 'Ставка рефинансирования (%)',
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