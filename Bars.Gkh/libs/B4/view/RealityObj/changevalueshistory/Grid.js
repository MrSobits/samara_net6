Ext.define('B4.view.realityobj.changevalueshistory.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ActionKind',
        'B4.store.realityobj.StructElHistory'
    ],

    alias: 'widget.changeValuesHistoryGrid',

    title: 'История изменений',

    initComponent: function () {
        var me = this,
            store = Ext.create("B4.store.realityobj.ChangeValuesHistory");

        var newArr = Ext.Array.filter(B4.enums.ActionKind.getItemsWithEmpty([null, '-']), function (item) {
            if ( item[1] === 'Удаление' ) { return false; } 
            return true;
        });

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата/время',
                    dataIndex: 'EntityDateChange',
                    format: 'd.m.Y H:i:s',
                    flex: 2,
                    filter: { xtype: 'datefield' }
                },
                { text: 'Пользователь', dataIndex: 'UserLogin', flex: 2, filter: { xtype: 'textfield' } },
                { text: 'IP', dataIndex: 'Ip', flex: 2, filter: { xtype: 'textfield' } },
                {
                    text: 'Действие',
                    dataIndex: 'EntityTypeChange',
                    flex: 2,
                    renderer: function (val) { return B4.enums.ActionKind.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: newArr,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                }
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