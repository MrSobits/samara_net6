Ext.define('B4.view.adminresp.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.adminrespgrid',
    requires: [
        'Ext.grid.Column',
        'Ext.grid.DateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    store: 'AdminResp',
    itemId: 'adminRespGrid',
    title: 'Административная ответственность',

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
                    dataIndex: 'SupervisoryOrgName',
                    flex: 1,
                    text: 'Наименование контролирующего органа'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AmountViolation',
                    flex: 1,
                    text: 'Количество выявленных нарушений'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateImpositionPenalty',
                    text: 'Дата наложения штрафа',
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumPenalty',
                    flex: 1,
                    text: 'Сумма штрафа'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePaymentPenalty',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата оплаты штрафа'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    flex: 1,
                    text: 'Документ',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Да</a>') : 'Нет';
                    }
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