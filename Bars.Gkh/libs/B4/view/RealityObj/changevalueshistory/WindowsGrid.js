Ext.define('B4.view.realityobj.changevalueshistory.WindowsGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ActionKind',
        'B4.store.realityobj.ChangeValuesHistoryDetail'
    ],

    alias: 'widget.changeValuesHistoryWindowsGrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.realityobj.ChangeValuesHistoryDetail');

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                { text: 'Наименование атрибута', dataIndex: 'PropertyName', flex: 2 },
                {
                    text: 'Старое значение', dataIndex: 'OldValue', flex: 2
                },
                {
                    text: 'Новое значание',
                    dataIndex: 'NewValue',
                    flex: 2
                }
            ],
            
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

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