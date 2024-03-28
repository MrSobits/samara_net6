Ext.define('B4.view.objectcr.PersonalAccountGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.objectcr.PersonalAccount',
        
        'B4.enums.TypeFinanceGroup'
    ],

    title: 'Лицевые счета',
    alias: 'widget.personalaccountgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.PersonalAccount');

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
                    dataIndex: 'FinanceGroup',
                    flex: 1,
                    text: 'Группа финансирования',
                    renderer: function (val) { return B4.enums.TypeFinanceGroup.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Account',
                    flex: 1,
                    text: 'Лицевой счет'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                },
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