Ext.define('B4.view.personalAccount.PublicControlGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gispersaccpubliccontrolgrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.personalaccount.PublicControl'
    ],

    title: 'Претензии граждан ("НК")',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.personalaccount.PublicControl');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CategoryName',
                    flex: 1,
                    text: 'Наименование категории',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 2,
                    text: 'Адрес объекта',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Territory',
                    flex: 1,
                    text: 'Наименование территории',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OrganizationName',
                    flex: 2,
                    text: 'Наименование организации',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StateName',
                    flex: 1,
                    text: 'Наименование статуса',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'CreatedDate',
                    flex: 1,
                    text: 'Дата создания',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'UpdateDate',
                    flex: 1,
                    text: 'Дата последнего изменения статуса',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [
                    {
                        xtype: 'buttongroup',
                        columns: 1,
                        items: [
                            { xtype: 'b4updatebutton' }
                        ]
                    }]
            },
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    }
});