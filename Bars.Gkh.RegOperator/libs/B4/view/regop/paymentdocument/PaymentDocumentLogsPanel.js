Ext.define('B4.view.regop.paymentdocument.PaymentDocumentLogsPanel', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'Ext.ux.grid.FilterBar',
        'B4.store.regop.paymentdocument.PaymentDocumentLogsStore'
    ],

    title: 'Прогресс документов на оплату',
    //cls: 'x-large-head',
    alias: 'widget.paymentdocumentlogspanel',

    closable: true,
    enableColumnHide: true,
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.paymentdocument.PaymentDocumentLogsStore');
        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    hidden: true,
                    width: 200,
                    text: 'Описание',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartTime',
                    width: 180,
                    text: 'Время начала',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y H:i:s'
                    }
                },
                { text: 'Выполнено', dataIndex: 'Count', flex: 1 },
                { text: 'Всего', dataIndex: 'AllCount', flex: 1 }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: []
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});