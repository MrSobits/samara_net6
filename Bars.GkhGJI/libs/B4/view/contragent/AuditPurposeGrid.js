Ext.define('B4.view.contragent.AuditPurposeGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.contragent.AuditPurpose'
    ],

    title: 'Даты проведения плановых проверок по целям',
    alias: 'widget.auditpurposegrid',
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.contragent.AuditPurpose');

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
                    dataIndex: 'AuditPurposeName',
                    flex: 1,
                    text: 'Цель'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LastInspDate',
                    text: 'Дата прошлой проверки',
                    format: 'd.m.Y',
                    width: 150
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
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