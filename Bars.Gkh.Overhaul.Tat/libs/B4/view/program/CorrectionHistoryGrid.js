Ext.define('B4.view.program.CorrectionHistoryGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ActionKind',
        'B4.store.program.CorrectionHistory'
    ],

    alias: 'widget.progcorrecthistorygrid',

    title: 'История изменений',

    initComponent: function () {
        var me = this,
            store = Ext.create("B4.store.program.CorrectionHistory");

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                { text: 'ООИ', dataIndex: 'EntityDescription', flex: 2, renderer: function (val) { return val.substring(val.indexOf('-')+1); } },
                {
                    xtype: 'datecolumn',
                    text: 'Дата/время',
                    dataIndex: 'EntityDateChange',
                    format: 'd.m.Y H:i:s',
                    flex: 2
                },
                { text: 'Пользователь', dataIndex: 'UserLogin', flex: 2 },
                { text: 'IP', dataIndex: 'Ip', flex: 2 },
                { text: 'Действие', dataIndex: 'EntityTypeChange', flex: 2, renderer: function (val) { return B4.enums.ActionKind.displayRenderer(val); } }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
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