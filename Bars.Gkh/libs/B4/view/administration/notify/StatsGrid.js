Ext.define('B4.view.administration.notify.StatsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.notifystatsgrid',

    requires: [
        'B4.enums.ButtonType',

        'B4.store.administration.notify.Stats',

        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'Ext.ux.grid.FilterBar'
    ],

    title: 'Статистика ответов на сообщения',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.notify.Stats');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Login',
                    header: 'Логин',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    header: 'Имя пользователя',
                    filter: { xtype: 'textfield' },
                    flex: 3
                },
                {
                    header: 'Дата ознакомления',
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    dataIndex: 'ObjectCreateDate',
                    width: 150,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y H:i:s'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ButtonType',
                    filter: true,
                    header: 'Ответ',
                    dataIndex: 'ClickButton',
                    flex: 1
                },
            ],
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            viewConfig: {
                loadMask: true
            },
            plugins: [
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }
            ],
        });

        me.callParent(arguments);
    }
});