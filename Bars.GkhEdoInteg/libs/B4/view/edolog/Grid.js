Ext.define('B4.view.edolog.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.edologGrid',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.YesNo'
    ],

    store: 'edolog.AppealCits',
    itemId: 'edologGrid',
    closable: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsEdo',
                    flex: 1,
                    text: 'Создано из ЭДО',
                    renderer: function(val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    text: 'Дата создания',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateActual',
                    text: 'Дата актуальности',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGji',
                    width: 170,
                    flex: 1,
                    text: 'Номер ГЖИ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Document',
                    width: 170,
                    flex: 1,
                    text: 'Документ',
                    filter: { xtype: 'textfield' }
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